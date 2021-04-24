using System.ComponentModel.DataAnnotations.Schema;

namespace LocalParks.Core.Domain.Shop
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Order Order { get; set; }
    }
}
