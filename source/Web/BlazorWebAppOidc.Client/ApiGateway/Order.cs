namespace BlazorWebAppOidc.Client
{
    public class Order
    {
        public Guid UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
