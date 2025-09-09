using System.Net.Http.Json;

namespace BlazorWebAppOidc.Client;

internal sealed class ClientProduct(HttpClient httpClient) : IProduct
{
    public async Task<IEnumerable<Product>> GetProductsAsync() =>
        await httpClient.GetFromJsonAsync<Product[]>("api/products") ??
            throw new IOException("No products!");

    public async Task<bool> CreateProductAsync(Product product)
    {
        var response = await httpClient.PostAsJsonAsync("api/products", product);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var response = await httpClient.PutAsJsonAsync($"api/products/{product.Id}", product);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(Guid productId)
    {
        var response = await httpClient.DeleteAsync($"api/products/{productId}");
        return response.IsSuccessStatusCode;
    }
}
