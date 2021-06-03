using LocalParks.Core.Models.Shop;
using System.Collections.Generic;

namespace LocalParks.Contracts
{
    public interface IOrderInfoService
    {
        public decimal CalculateTotal(ICollection<OrderItemModel> items);
        public decimal CalculateSubtotal(OrderItemModel item);
        public string FormatImageUri(string imageId);
    }
}
