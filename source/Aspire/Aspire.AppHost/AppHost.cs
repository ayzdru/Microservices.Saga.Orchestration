using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImageTag("17")
    .WithContainerName("postgres")
    .WithVolume("postgresql_data", "/var/lib/postgresql/data")
    .WithEndpoint(
        "tcp",
        e =>
        {
            e.Port = 5432;
            e.TargetPort = 5432;
            e.IsProxied = false;
            e.IsExternal = false;
        })
    //.WithEndpoint(name: "postgresendpoint", scheme: "tcp", port: 5432, targetPort: 5432, isProxied: false)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithPgAdmin(pgAdmin =>
    pgAdmin.WithContainerName("pgAdmin")
    .WithImageTag("9.8.0")
    .WithEndpoint(
        "http",
        e =>
        {
            e.Port = 5050;
            e.TargetPort = 80;
            e.IsProxied = false;
            e.IsExternal = false;
        })
    .WithVolume("pgadmin_data", "/var/lib/pgadmin").WithLifetime(ContainerLifetime.Persistent));
var identityDatabaseName = "Identity";
var creationScript = $$"""    
    CREATE DATABASE '{{identityDatabaseName}}';
    CREATE USER administrator WITH SUPERUSER PASSWORD 'Password1@_';
    """;
var identityDB = postgres.AddDatabase(identityDatabaseName).WithCreationScript(creationScript);
var orderDB = postgres.AddDatabase("Order");
var paymentDB = postgres.AddDatabase("Payment");
var productDB = postgres.AddDatabase("Product");
var orchestrationDB = postgres.AddDatabase("Orchestration");

var consul = builder.AddContainer("consul", "consul")
    .WithImageTag("1.15.4")
    .WithContainerName("consul")
    .WithVolume("consul_data", "/consul/data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithEndpoint(
        "tcp",
        e =>
        {
            e.Port = 8500;
            e.TargetPort = 8500;
            e.IsProxied = false;
            e.IsExternal = false;
        });
//.WithEndpoint(port: 8500, targetPort: 8500, scheme: "tcp", isProxied: false);

var rabbitmqUsername = builder.AddParameter("admin", secret: true);
var rabbitmqPassword = builder.AddParameter("Password1", secret: true);
var rabbitmq = builder.AddRabbitMQ("rabbitmq", rabbitmqUsername, rabbitmqPassword)
    .WithContainerName("rabbitmq")
    .WithImageTag("4.1.4-management")
    .WithDataVolume("rabbitmq_data")
    .WithEndpoint(
        "tcp",
        e =>
        {
            e.Port = 5672;
            e.TargetPort = 5672;
            e.IsProxied = false;
            e.IsExternal = false;
        })
    .WithEndpoint(
        "management",
        e =>
        {
            e.Port = 15672;
            e.TargetPort = 15672;
            e.IsProxied = false;
            e.IsExternal = false;
        })
    //.WithEndpoint(name: "rabbitmq",port: 5672, targetPort: 5672, isProxied: false)
    //.WithEndpoint(name: "rabbitmq-management", port: 15672, targetPort: 15672, isProxied: false)
    //.WithManagementPlugin(port: 15672)
    .WithLifetime(ContainerLifetime.Persistent);


var identityMigrationService = builder.AddProject<Projects.Identity>("identitymigration", "EFMigration")
    .WithReference(identityDB)
    .WaitFor(identityDB);

var identityServer = builder.AddProject<Projects.Identity_STS_Identity>("identityserver");
identityServer.WithHttpHealthCheck("/health")
      .WaitForCompletion(identityMigrationService, 0)
      .WithReference(identityDB)
      .WaitFor(identityDB)
      .WaitFor(consul);

var identityServerAdminApi = builder.AddProject<Projects.Identity_Api>("identityserveradminapi");
identityServerAdminApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(identityDB)
          .WaitFor(identityDB);


var identityServerAdminWeb = builder.AddProject<Projects.Identity>("identityserveradminweb", "Identity");
identityServerAdminWeb.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(identityServerAdminApi)
          .WaitFor(identityServerAdminApi)
          .WithReference(identityDB)
          .WaitFor(identityDB);





var productMigrationService = builder.AddProject<Projects.Product_API>("productmigration", "EFMigration")
    .WithReference(productDB)
    .WaitFor(productDB);

var productApi = builder.AddProject<Projects.Product_API>("productapi");
productApi.WithHttpHealthCheck("/health")
           .WaitForCompletion(productMigrationService, 0)
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(productDB)
          .WaitFor(productDB);

var orderApi = builder.AddProject<Projects.Order_API>("orderapi");
orderApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(orderDB)
          .WaitFor(orderDB);

var paymentApi = builder.AddProject<Projects.Payment_API>("paymentapi");
paymentApi.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(paymentDB)
          .WaitFor(paymentDB);

var ocelotApiGateway = builder.AddProject<Projects.Ocelot_ApiGateway_Web>("ocelotapigateway");
ocelotApiGateway.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(productApi)
          .WaitFor(productApi)
          .WithReference(orderApi)
          .WaitFor(orderApi)
          .WithReference(paymentApi)
          .WaitFor(paymentApi);

var web = builder.AddProject<Projects.BlazorWebAppOidc>("web");
web.WithHttpHealthCheck("/health")
   .WaitFor(consul)
   .WithReference(identityServer)
   .WaitFor(identityServer)
   .WithReference(ocelotApiGateway)
   .WaitFor(ocelotApiGateway);

builder.Build().Run();
