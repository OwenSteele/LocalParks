using LocalParks.Core.Shop;
using LocalParks.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IShopManager
    {
        Task<OrderModel> SubmitOrderAsync(OrderModel model, string username);
    }
}