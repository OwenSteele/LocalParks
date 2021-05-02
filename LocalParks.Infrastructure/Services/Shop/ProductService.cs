using AutoMapper;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models.Shop;
using LocalParks.Data;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services.Shop
{
    public class ProductService : IProductService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public ProductService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ProductModel[]> GetAllProductsAsync()
        {
            return _mapper.Map<ProductModel[]>(await _parkRepository.GetAllProductsAsync());
        }
        public async Task<ProductModel[]> GetShopProductsAsync()
        {
            return _mapper.Map<ProductModel[]>(await _parkRepository.GetShopProductsAsync());
        }

        public async Task<ProductModel[]> GetMembershipProductsAsync()
        {
            return _mapper.Map<ProductModel[]>(await _parkRepository.GetMembershipProductsAsync());
        }
    }
}
