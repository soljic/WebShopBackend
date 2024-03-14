namespace API.DTOs
{
    public class CommandProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile Picture { get; set; } // ovo omogućuje spremanje slike u aplikaciju
        public string ProductType { get; set; }
        public string ProductBrand { get; set; }
    }
}
