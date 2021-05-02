using LocalParks.Core.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Core.Contracts
{
    public interface IProductService
    {
        Task<ProductModel[]> GetAllProductsAsync();
        Task<ProductModel[]> GetMembershipProductsAsync();
        Task<ProductModel[]> GetShopProductsAsync();
    }
}