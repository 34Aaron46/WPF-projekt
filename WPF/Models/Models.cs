namespace BoostBayWpf.Models
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? SpecValue { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = "Új";
        public ProductDto? Product { get; set; }

        public decimal Total
        {
            get
            {
                if (Product == null)
                {
                    return 0;
                }
                return Product.Price * Quantity;
            }
        }
    }
}
