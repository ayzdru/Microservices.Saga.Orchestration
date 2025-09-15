using Consul;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Aspire.ServiceDefaults
{
    public sealed class DiscoveryHttpDelegatingHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        public DiscoveryHttpDelegatingHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var originalHost = request.RequestUri.Host;
            var consulClient = (IConsulClient)_serviceProvider.GetService(typeof(IConsulClient));
            if (consulClient == null)
                throw new InvalidOperationException("IConsulClient is not registered in the service provider.");
            var services = await consulClient.Catalog.Service(originalHost, cancellationToken);
            if (services.Response != null && services.Response.Length != 0)
            {              
                var service = services.Response[0];
                var newUriBuilder = new UriBuilder(request.RequestUri)
                {
                    Host = service.ServiceAddress,
                    Port = service.ServicePort
                };
                request.RequestUri = newUriBuilder.Uri;
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
