using AutoMapper;
using LocalParks.Core;
using LocalParks.Core.Shop;
using LocalParks.Data;
using LocalParks.Models.Shop;
using LocalParks.Services.Shop;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ShopManager : IShopManager
    {
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly IOrderCreationService _creationService;
        private readonly IOrderService _orderService;

        public ShopManager(UserManager<LocalParksUser> userManager,
            IOrderCreationService creationService,
            IOrderService orderService)
        {
            _userManager = userManager;
            _creationService = creationService;
            _orderService = orderService;
        }
        public async Task<OrderModel> SubmitOrderAsync(OrderModel model, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return null;

            if (!await _creationService.CreateOrderAsync(model, user))
                return null;

            var result = await _orderService.GetOrderNumberAsync(model.OrderNumber);

            return result;
        }
    }
}
