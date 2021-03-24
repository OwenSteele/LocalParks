namespace LocalParks.Core.Shop
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageId { get; set; }
        public ProductCategoryType Category { get; set; }
    }
}
