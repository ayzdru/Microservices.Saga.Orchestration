using BlazorWebAppOidc.Client;
using System.Net.Http.Json;

namespace BlazorWebAppOidc;

internal sealed class ServerProduct(IHttpClientFactory clientFactory) : IProduct
{
    public async Task<bool> CreateProductAsync(Product product)
    {
        var client = clientFactory.CreateClient("ApiGateway");
        var response = await client.PostAsJsonAsync("/products", product);
        response.EnsureSuccessStatusCode();

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var client = clientFactory.CreateClient("ApiGateway");
        var response = await client.DeleteAsync($"/products/{productId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/products");
        var client = clientFactory.CreateClient("ApiGateway");
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<Product[]>() ??
            throw new IOException("No products!");
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var client = clientFactory.CreateClient("ApiGateway");
        var response = await client.PutAsJsonAsync($"/products/{product.Id}", product);
        response.EnsureSuccessStatusCode();

        return response.IsSuccessStatusCode;
    }
}
