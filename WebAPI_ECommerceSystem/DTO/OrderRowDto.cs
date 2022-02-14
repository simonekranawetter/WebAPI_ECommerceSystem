namespace WebAPI_ECommerceSystem.DTO
{
    public class OrderRowDto
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public OrderProductDto Product { get; set; }
    }
}
