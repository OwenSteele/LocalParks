using AutoMapper;
using LocalParks.Core;
using LocalParks.Core.Shop;
using LocalParks.Data;
using LocalParks.Models.Shop;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ShopService : IShopService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<LocalParksUser> _userManager;

        public ShopService(IParkRepository parkRepository, IMapper mapper,
            UserManager<LocalParksUser> userManager)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<Product[]> GetAllProductsAsync()
        {
            return await _parkRepository.GetAllProductsAsync();
        }
        public async Task<Product[]> GetShopProductsAsync()
        {
            return await _parkRepository.GetShopProductsAsync();
        }

        public async Task<Product[]> GetMembershipProductsAsync()
        {
            return await _parkRepository.GetMembershipProductsAsync();
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
        public async Task<OrderModel> SubmitNewOrderAsync(OrderModel model, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            var order = _mapper.Map<Order>(model);

            if (order.Items == null || !order.Items.Any()) throw new Exception("items not mapped");

            if (order.DateCreated == DateTime.MinValue)
                order.DateCreated = DateTime.Now;

            order.OrderNumber = order.OrderNumber.Replace("-", "").Replace(":", "").Replace("_", "");

            if (order.DeliveryDate == DateTime.MinValue)
                order.DeliveryDate = order.DateCreated.AddDays(3);

            order.User = user;

            _parkRepository.Add(order);

            if (!await _parkRepository.SaveChangesAsync()) return null;

            var result = await _parkRepository.GetOrderByNumberAsync(order.OrderNumber);

            return _mapper.Map<OrderModel>(result);
        }
    }
}
