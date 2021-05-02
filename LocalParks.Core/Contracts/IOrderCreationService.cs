using LocalParks.Core.Domain.User;
using LocalParks.Core.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IOrderCreationService
    {
        Task<bool> CreateOrderAsync(OrderModel model, LocalParksUser user);
    }
}