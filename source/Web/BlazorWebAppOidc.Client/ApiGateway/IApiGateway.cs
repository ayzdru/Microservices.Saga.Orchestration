
namespace BlazorWebAppOidc.Client;

public interface IApiGateway
{
    //Product
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<bool> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(Guid productId);


    //Order
    Task<ApiResult<string>> CreateOrderAsync(Order order);
}
