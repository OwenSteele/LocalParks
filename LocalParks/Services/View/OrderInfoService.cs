using LocalParks.Contracts;
using LocalParks.Core.Models.Shop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalParks.Services.View
{
    public class OrderInfoService : IOrderInfoService
    {
        public decimal CalculateTotal(ICollection<OrderItemModel> items) => Math.Round(items.Sum(i => i.UnitPrice * i.Quantity), 2);
        public decimal CalculateSubtotal(OrderItemModel item) => Math.Round(item.Quantity * item.UnitPrice, 2);
        public string FormatImageUri(string imageId) => $"{imageId}.jpg";
    }
}
