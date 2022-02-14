namespace WebAPI_ECommerceSystem.DTO
{
    public class AddProductDto
    {
        public string ArticleNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
