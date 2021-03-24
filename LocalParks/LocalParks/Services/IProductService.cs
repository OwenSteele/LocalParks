using LocalParks.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IProductService
    {
        Task<ProductModel[]> GetAllProductsAsync();
        Task<ProductModel[]> GetMembershipProductsAsync();
        Task<ProductModel[]> GetShopProductsAsync();
    }
}