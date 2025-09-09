using Product.Infrastructure.Data;
using System.Diagnostics;

namespace Product.API.Services
{
    public class MigrationService( IServiceProvider serviceProvider,  IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";
        private static readonly ActivitySource s_activitySource = new(ActivitySourceName);
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var initialiser = scope.ServiceProvider.GetRequiredService<ProductDbContextInitialiser>();
                await initialiser.InitialiseAsync(cancellationToken);
                await initialiser.SeedAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                activity?.AddException(ex);
                throw;
            }

            hostApplicationLifetime.StopApplication();
        }
    }
}
