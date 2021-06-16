using LocalParks.Core.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IOrderService
    {
        Task<OrderModel[]> GetAllOrdersAsync();
        Task<OrderModel> GetOrderAsync(int orderId);
        Task<OrderModel[]> GetUserOrdersAsync(string username);
        Task<OrderModel> GetOrderNumberAsync(string orderNumber);
    }
}