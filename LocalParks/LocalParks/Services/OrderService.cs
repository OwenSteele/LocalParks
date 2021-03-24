using AutoMapper;
using LocalParks.Data;
using LocalParks.Models.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class OrderService : IOrderService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public OrderService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<OrderModel[]> GetAllOrdersAsync()
        {
            return _mapper.Map<OrderModel[]>(await _parkRepository.GetAllOrdersAsync());
        }
        public async Task<OrderModel[]> GetUserOrdersAsync(string username)
        {
            return _mapper.Map<OrderModel[]>(await _parkRepository.GetOrdersByUsernameAsync(username));
        }
        public async Task<OrderModel> GetOrderAsync(int orderId)
        {
            return _mapper.Map<OrderModel>(await _parkRepository.GetOrderByIdAsync(orderId));
        }
        public async Task<OrderModel> GetOrderNumberAsync(string orderNumber)
        {
            return _mapper.Map<OrderModel>(await _parkRepository.GetOrderByNumberAsync(orderNumber));
        }
        
    }
}
