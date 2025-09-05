using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres", port: 57869)
    .WithImageTag("17")
    .WithVolume("postgresql_data", "/var/lib/postgresql/data")
    .WithLifetime(ContainerLifetime.Persistent).WithPgAdmin(pgAdmin => pgAdmin.WithImageTag("9.8.0").WithHostPort(5050).WithVolume("pgadmin_data", "/var/lib/pgadmin").WithLifetime(ContainerLifetime.Persistent));
var databaseName = "idsrv4";
var creationScript = $$"""
    -- Create the database
    CREATE DATABASE {{databaseName}};
    CREATE USER idsrv4 WITH SUPERUSER PASSWORD 'Local@Db';
    """;
var identityServer4DB = postgres.AddDatabase(databaseName).WithCreationScript(creationScript);


var consul = builder.AddContainer("consul", "consul")
    .WithImageTag("1.15.4")
    .WithVolume("consul_data", "/consul/data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHttpEndpoint(8500, targetPort: 8500);



var efmigration = builder.AddProject<Projects.Identity>("efmigration", "EFMigration")
    .WithReference(identityServer4DB)
    .WaitFor(identityServer4DB);

var identityServer = builder.AddProject<Projects.Identity_STS_Identity>("identityserver");
identityServer.WithHttpHealthCheck("/health")
      .WaitForCompletion(efmigration, 0)
      .WithReference(identityServer4DB)
      .WaitFor(identityServer4DB)
      .WaitFor(consul);

var identityServerAdminApi = builder.AddProject<Projects.Identity_Api>("identityserveradminapi");
identityServerAdminApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(identityServer4DB)              
          .WaitFor(identityServer4DB);


var identityServerAdminWeb = builder.AddProject<Projects.Identity>("identityserveradminweb", "Identity");
identityServerAdminWeb.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(identityServerAdminApi)
          .WaitFor(identityServerAdminApi)
          .WithReference(identityServer4DB)
          .WaitFor(identityServer4DB);



var ocelotApiGateway = builder.AddProject<Projects.Product_API>("ocelotapigateway");
ocelotApiGateway.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer);


var productApi = builder.AddProject<Projects.Product_API>("productapi");
productApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer);

var orderApi = builder.AddProject<Projects.Order_API>("orderapi");
orderApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer);

var paymentApi = builder.AddProject<Projects.Payment_API>("paymentapi");
paymentApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer);

var web = builder.AddProject<Projects.Microservices_Saga_Orchestration_Web>("web");
web.WithExternalHttpEndpoints()
          .WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer);

builder.Build().Run();
