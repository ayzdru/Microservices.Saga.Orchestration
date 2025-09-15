# Microservices.Saga.Orchestration
.NET Mikroservis Mimari Orchestration-Based Saga Pattern ile Transactional Outbox ve Inbox (Clean Architecture, Domain-Driven Design, Blazor, Identity Server, MassTransit, RabbitMQ, PostgreSQL, Aspire, Ocelot Api Gateway, Consul Service Discovery)


EF Migration Komutları

dotnet ef migrations add "Initial" --context ProductDbContext --project source\Services\Product\Product.Infrastructure --startup-project source\Services\Product\Product.API --output-dir Data\Migrations

dotnet ef migrations add "Initial" --context OrchestrationDbContext --project source\Services\Orchestration\Orchestration.Infrastructure --startup-project source\Services\Orchestration\Orchestration.Service --output-dir Data\Migrations

dotnet ef migrations add "Initial" --context OrderDbContext --project source\Services\Order\Order.Infrastructure --startup-project source\Services\Order\Order.API --output-dir Data\Migrations

dotnet ef migrations add "Initial" --context PaymentDbContext --project source\Services\Payment\Payment.Infrastructure --startup-project source\Services\Payment\Payment.API --output-dir Data\Migrations
