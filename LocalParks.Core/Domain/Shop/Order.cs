using LocalParks.Core.Domain.User;
using System;
using System.Collections.Generic;

namespace LocalParks.Core.Domain.Shop
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderNumber { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public LocalParksUser User { get; set; }
    }
}