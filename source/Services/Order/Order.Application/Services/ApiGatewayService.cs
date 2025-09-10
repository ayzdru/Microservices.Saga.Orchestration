using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public record Product(Guid Id, string Name, decimal Price, int Stock);

    public sealed class ApiGatewayService(HttpClient httpClient, ILogger<ApiGatewayService> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<ApiGatewayService> _logger = logger;

        public async Task<List<Product>?> GetProductsByIdsAsync(List<Guid> productIds, CancellationToken cancellationToken)
        {
            var response = await _httpClient.PostAsJsonAsync("/products/get-products-by-ids", productIds, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch products. Status code: {StatusCode}", response.StatusCode);
                return null;
            }

            var products = await response.Content.ReadFromJsonAsync<List<Product>>(cancellationToken: cancellationToken);
            _logger.LogInformation("Returning products: {@Products}", products);

            return products;
        }
    }
}
