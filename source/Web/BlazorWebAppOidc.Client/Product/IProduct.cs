namespace BlazorWebAppOidc.Client;

public interface IProduct
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<bool> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productId);
}
