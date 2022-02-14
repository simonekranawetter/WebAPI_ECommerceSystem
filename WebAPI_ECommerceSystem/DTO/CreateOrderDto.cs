namespace WebAPI_ECommerceSystem.DTO
{
    public class CreateOrderDto
    {
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<CreateOrderRowDto> OrderRows { get; set; }
    }
}
