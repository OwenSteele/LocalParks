using LocalParks.Core.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IShopManager
    {
        Task<OrderModel> SubmitOrderAsync(OrderModel model, string username);
    }
}