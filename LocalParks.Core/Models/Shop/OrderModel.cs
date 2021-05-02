using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LocalParks.Core.Models.Shop
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public DateTime DeliveryDate { get; set; }
        [Required]
        [MinLength(5)]
        public string OrderNumber { get; set; }
        public ICollection<OrderItemModel> Items { get; set; }
        public string Username { get; set; }

        public decimal Total { get { return Math.Round(Items.Sum(i => i.UnitPrice * i.Quantity), 2); } }
    }
}
