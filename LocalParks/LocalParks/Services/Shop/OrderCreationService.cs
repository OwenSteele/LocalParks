using AutoMapper;
using LocalParks.Core;
using LocalParks.Core.Shop;
using LocalParks.Data;
using LocalParks.Models.Shop;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services.Shop
{
    public class OrderCreationService : IOrderCreationService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public OrderCreationService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<bool> CreateOrderAsync(OrderModel model, LocalParksUser user)
        {
            var order = _mapper.Map<Order>(model);

            if (order.Items == null || !order.Items.Any()) throw new Exception("items not mapped");

            if (order.DateCreated == DateTime.MinValue)
                order.DateCreated = DateTime.Now;

            order.OrderNumber = order.OrderNumber.Replace("-", "").Replace(":", "").Replace("_", "");

            if (order.DeliveryDate == DateTime.MinValue)
                order.DeliveryDate = order.DateCreated.AddDays(3);

            order.User = user;

            _parkRepository.Add(order);

            return await _parkRepository.SaveChangesAsync();
        }
    }
}
