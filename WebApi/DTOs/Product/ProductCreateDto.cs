namespace WebApi.DTOs.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public IFormFile Photo { get; set; }
        public int CategoryId { get; set; }
    }
}