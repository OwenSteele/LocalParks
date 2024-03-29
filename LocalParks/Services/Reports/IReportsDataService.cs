﻿using LocalParks.Core.Domain;
using LocalParks.Core.Domain.Shop;
using System.Threading.Tasks;

namespace LocalParks.Services.Reports
{
    public interface IReportsDataService
    {
        Task<Order[]> GetAllOrders();
        Task<ParkEvent[]> GetAllParkEvents();
        Task<Park[]> GetAllParks();
        Task<SportsClub[]> GetAllSportsClubs();
        Task<Supervisor[]> GetAllSupervisors();
        Task<Product[]> GetAllProducts();
        Task<int> SportCount();
        Task<string> SportName(int pos);
    }
}