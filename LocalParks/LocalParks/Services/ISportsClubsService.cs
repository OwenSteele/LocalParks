﻿using LocalParks.Core;
using LocalParks.Models;
using LocalParks.Services.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface ISportsClubsService : IFilterService
    {
        Task<SportsClubModel[]> GetAllSportsClubModelsAsync();
        Task<SportsClubModel[]> GetSearchedSportsClubModelsAsync(string searchTerm = null, string parkId = null, string sportType = null);
        IEnumerable<SelectListItem> GetSportListItems();
        Task<SportsClubModel> GetSportsClubModelAsync(int clubId);
        Task<SportsClubModel[]> GetSportsClubModelsByParkAsync(int parkId);
        Task<SportsClubModel[]> GetSportsClubModelsBySportAsync(int parkId, SportType sportType);
        Task<bool> CheckParkExistsAsync(int parkId, string clubName = null);
        ICollection<string> GetAllSports();
    }
}