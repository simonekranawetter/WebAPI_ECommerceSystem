using WebAPI_ECommerceSystem.Entities;

namespace WebAPI_ECommerceSystem.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<OrderRowDto> OrderRows { get; set; }
    }
}
