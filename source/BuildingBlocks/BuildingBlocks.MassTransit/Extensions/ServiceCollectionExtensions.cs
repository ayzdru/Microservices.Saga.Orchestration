using BuildingBlocks.MassTransit.Settings;
using MassTransit.MongoDbIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BuildingBlocks.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDbOutbox(this IServiceCollection services, IConfigurationManager configuration)
        {
            var section = configuration.GetSection("MongoDb");
            var settings = new MongoDbSettings
            {
                ConnectionString = section["ConnectionString"],
                Database = section["Database"]
            };
            var mongoClientSettings = MongoClientSettings.FromConnectionString(settings.ConnectionString);
            var pack = new ConventionPack
{
    new EnumRepresentationConvention(BsonType.String)
};

            ConventionRegistry.Register("EnumStringConvention", pack, _ => true);

            services.AddSingleton<IMongoClient>(new MongoClient(mongoClientSettings));

            services.AddSingleton<IMongoDatabase>(provider => provider
                .GetRequiredService<IMongoClient>()
                .GetDatabase(settings.Database)
            );
            return services;
        }
    }
}
