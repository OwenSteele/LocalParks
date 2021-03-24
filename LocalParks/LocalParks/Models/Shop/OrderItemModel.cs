using System;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Models.Shop
{
    public class OrderItemModel
    {
        [Required]
        public int ItemId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public int OrderId { get; set; }

        public string ProductName { get; set; }

        public string ImageId { get; set; }

        public decimal Subtotal { get { return Math.Round(Quantity * UnitPrice, 2); } }
        public string ImageUri { get => $"{ImageId}.jpg"; }
    }
}
