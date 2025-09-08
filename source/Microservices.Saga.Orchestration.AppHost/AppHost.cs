using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres", port: 57869)
    .WithImageTag("17")
    .WithVolume("postgresql_data", "/var/lib/postgresql/data")
    .WithLifetime(ContainerLifetime.Persistent).WithPgAdmin(pgAdmin => pgAdmin.WithImageTag("9.8.0").WithHostPort(5050).WithVolume("pgadmin_data", "/var/lib/pgadmin").WithLifetime(ContainerLifetime.Persistent));
var identityDatabaseName = "Identity";
var creationScript = $$"""    
    CREATE DATABASE '{{identityDatabaseName}}';
    CREATE USER administrator WITH SUPERUSER PASSWORD 'Password1@_';
    """;
var identityDB = postgres.AddDatabase(identityDatabaseName).WithCreationScript(creationScript);
var orderDB = postgres.AddDatabase("Order");
var paymentDB = postgres.AddDatabase("Payment");
var productDB = postgres.AddDatabase("Product");

var consul = builder.AddContainer("consul", "consul")
    .WithImageTag("1.15.4")
    .WithVolume("consul_data", "/consul/data")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithHttpEndpoint(8500, targetPort: 8500);



var efmigration = builder.AddProject<Projects.Identity>("efmigration", "EFMigration")
    .WithReference(identityDB)
    .WaitFor(identityDB);

var identityServer = builder.AddProject<Projects.Identity_STS_Identity>("identityserver");
identityServer.WithHttpHealthCheck("/health")
      .WaitForCompletion(efmigration, 0)
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



var ocelotApiGateway = builder.AddProject<Projects.Product_API>("ocelotapigateway");
ocelotApiGateway.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer);


var productApi = builder.AddProject<Projects.Product_API>("productapi");
productApi.WithHttpHealthCheck("/health")
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

var web = builder.AddProject<Projects.BlazorWebAppOidc>("web");
       web.WithHttpHealthCheck("/health")
          .WaitFor(consul)
          .WithReference(identityServer)
          .WaitFor(identityServer)
          .WithReference(ocelotApiGateway)
          .WaitFor(ocelotApiGateway);

builder.Build().Run();
