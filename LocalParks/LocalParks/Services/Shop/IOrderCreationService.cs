using LocalParks.Core;
using LocalParks.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services.Shop
{
    public interface IOrderCreationService
    {
        Task<bool> CreateOrderAsync(OrderModel model, LocalParksUser user);
    }
}