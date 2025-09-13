using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Aspire.AppHost;
static class MongoDBContainerImageTags
{
    /// <remarks>docker.io</remarks>
    public const string Registry = "docker.io";

    /// <remarks>library/mongo</remarks>
    public const string Image = "library/mongo";

    /// <remarks>8.0</remarks>
    public const string Tag = "8.0";

    /// <remarks>docker.io</remarks>
    public const string MongoExpressRegistry = "docker.io";

    /// <remarks>library/mongo-express</remarks>
    public const string MongoExpressImage = "library/mongo-express";

    /// <remarks>1.0</remarks>
    public const string MongoExpressTag = "1.0";
}
public static class MongoReplicaSetBuilderExtensions
{    // Internal port is always 27017.
    private const int DefaultContainerPort = 27017;

    private const string UserEnvVarName = "MONGO_INITDB_ROOT_USERNAME";
    private const string PasswordEnvVarName = "MONGO_INITDB_ROOT_PASSWORD";
    public static IResourceBuilder<MongoDBServerResource> AddMongoDBWithReplicaSet(this IDistributedApplicationBuilder builder,
       string name,
       int? port = null,
       IResourceBuilder<ParameterResource>? userName = null,
       IResourceBuilder<ParameterResource>? password = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrEmpty(name);

        var passwordParameter = password?.Resource ?? ParameterResourceBuilderExtensions.CreateDefaultPasswordParameter(builder, $"{name}-password", special: false);

        var mongoDBContainer = new MongoDBServerResource(name, userName?.Resource, passwordParameter);

        string? connectionString = null;

        builder.Eventing.Subscribe<ConnectionStringAvailableEvent>(mongoDBContainer, async (@event, ct) =>
        {
            connectionString = await mongoDBContainer.ConnectionStringExpression.GetValueAsync(ct).ConfigureAwait(false) + "&directConnection=true";

            if (connectionString == null)
            {
                throw new DistributedApplicationException($"ConnectionStringAvailableEvent was published for the '{mongoDBContainer.Name}' resource but the connection string was null.");
            }
        });     
        var healthCheckKey = $"{name}_check";
        // cache the client so it is reused on subsequent calls to the health check
        IMongoClient? client = null;
        builder.Services.AddHealthChecks()
            .AddMongoDb(
                sp => client ??= new MongoClient(connectionString ?? throw new InvalidOperationException("Connection string is unavailable")),
                name: healthCheckKey);

        return builder
            .AddResource(mongoDBContainer)
            .WithEndpoint(port: port, targetPort: DefaultContainerPort, name: "tcp")
            .WithImage(MongoDBContainerImageTags.Image, MongoDBContainerImageTags.Tag)
            .WithImageRegistry(MongoDBContainerImageTags.Registry)
            .WithEnvironment(context =>
            {
                context.EnvironmentVariables[UserEnvVarName] = ReferenceExpression.Create($"{mongoDBContainer.UserNameParameter}");
                context.EnvironmentVariables[PasswordEnvVarName] = mongoDBContainer.PasswordParameter!;
            })
            .WithHealthCheck(healthCheckKey);
    }

    /// <summary>
    /// Configures the MongoDB server to use a replica set.
    /// </summary>
    /// <param name="builder">The <see cref="IResourceBuilder{T}"/>.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{MongoDBServerResource}"/>.</returns>
    public static IResourceBuilder<MongoDBServerResource> WithReplicaSet(this IResourceBuilder<MongoDBServerResource> builder, string contextPath)
    {
        return builder
            .WithDockerfile(contextPath, "Mongo.Dockerfile")
            .WithArgs("--replSet", "rs0", "--bind_ip_all", "--keyFile", "/etc/mongo-keyfile");
    }

    /// <summary>
    /// Adds a MongoDB replica set resource to the application model.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the resource. This name will be used as the connection string name when referenced in a dependency.</param>
    /// <param name="mongoDbServerResource">The <see cref="MongoDBServerResource"/> to use for the MongoDB server.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<MongoReplicaSetResource> AddMongoReplicaSet(this IDistributedApplicationBuilder builder, string name, IResourceWithConnectionString mongoDbServerResource)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(mongoDbServerResource);

        var mongoReplicaSetResource = new MongoReplicaSetResource(name, mongoDbServerResource);

        MongoClientSettings? mongoClientSettings = null;

        builder.Eventing.Subscribe<ConnectionStringAvailableEvent>
        (
            mongoReplicaSetResource,

            async (@event, ct) =>
            {
                var connectionString =
                    await mongoReplicaSetResource.ConnectionStringExpression.GetValueAsync(ct).ConfigureAwait(false) ??
                    throw new DistributedApplicationException
                    (
                        $"ConnectionStringAvailableEvent was published for the '{mongoReplicaSetResource.Name}' resource but the connection string was null."
                    );

                var options = MongoClientSettings.FromConnectionString(connectionString);
                options.LoggingSettings = new LoggingSettings(@event.Services.GetRequiredService<ILoggerFactory>());

                mongoClientSettings = options;
            }
        );

        // the mongodb health check fails to connect because it has not the correct settings for the replica set
        foreach (var annotation in mongoDbServerResource.Annotations.OfType<HealthCheckAnnotation>().ToList())
        {
            mongoDbServerResource.Annotations.Remove(annotation);

            builder.Services.Configure<HealthCheckServiceOptions>
            (
                options =>
                {
                    var mongoDbServerHealthCheck = options.Registrations
                        .FirstOrDefault(x => x.Name == annotation.Key);

                    if (mongoDbServerHealthCheck is not null)
                    {
                        options.Registrations.Remove(mongoDbServerHealthCheck);
                    }
                }
            );
        }

        var healthCheckKey = $"{name}_check";
        builder.Services.AddHealthChecks()
            .Add
            (
                new HealthCheckRegistration
                (
                    healthCheckKey,
                    sp => new MongoReplicaSetHealthCheck(mongoClientSettings!),
                    failureStatus: null,
                    tags: null,
                    timeout: null
                )
            );

        return builder
            .AddResource(mongoReplicaSetResource)
            .WithHealthCheck(healthCheckKey)
            .WithConnectionStringRedirection(mongoDbServerResource);
    }
}