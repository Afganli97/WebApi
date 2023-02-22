namespace WebApi.DTOs.Product
{
    public class ProductReturnDto
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double SalePrice { get; set; }
        public string CategoryName { get; set; }
        public string FullPath { get; set; }
    }
}