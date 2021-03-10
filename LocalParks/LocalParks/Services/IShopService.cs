using LocalParks.Core.Shop;
using LocalParks.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IShopService
    {
        Task<Product[]> GetAllProductsAsync();
        Task<OrderModel[]> GetAllOrdersAsync();
        Task<OrderModel[]> GetUserOrdersAsync(string username);
        Task<OrderModel> GetOrderAsync(int orderId);
        Task<OrderModel> SubmitNewOrderAsync(OrderModel model, string username);
    }
}