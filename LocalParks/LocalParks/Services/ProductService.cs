using AutoMapper;
using LocalParks.Data;
using LocalParks.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services
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
