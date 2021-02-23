using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services.Combined;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class SportsClubsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public SportsClubsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<SportsClubModel[]> GetAllSportsClubModelsAsync(string sortBy = null)
        {
            var results = _mapper.Map<SportsClubModel[]>(await _parkRepository.GetAllSportsClubsAsync());

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(results, sortBy);

            return results;
        }
        public async Task<SportsClubModel[]> GetSearchedSportsClubModelsAsync(
            string searchTerm = null,
            string parkId = null,
            string sportType = null,
            string sortBy = null)
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                results = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

                if (!results.Any()) return null;
            }
            if (!string.IsNullOrWhiteSpace(parkId))
            {
                var park = int.Parse(parkId);

                results = results.Where(p =>
                p.Park.ParkId == park).ToArray();

                if (!results.Any()) return null;
            }
            if (!string.IsNullOrWhiteSpace(sportType))
            {
                results = results.Where(p =>
                p.Sport.ToString().ToLower() == sportType).ToArray();

                if (!results.Any()) return null;
            }

            var models = _mapper.Map<SportsClubModel[]>(results);

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(models, sortBy);

            return models;
        }
        public async Task<SportsClubModel> GetSportsClubModelAsync(int clubId, int? parkId = null)
        {
            var result = await _parkRepository.GetSportsClubByIdAsync(clubId, parkId);

            if (result == null) return null;

            return _mapper.Map<SportsClubModel>(result);
        }
        public async Task<SportsClubModel[]> GetSportsClubModelsByParkAsync(int parkId)
        {
            var results = await _parkRepository.GetSportsClubsByParkIdAsync(parkId);

            if (!results.Any()) return null;

            return _mapper.Map<SportsClubModel[]>(results);
        }
        public async Task<SportsClubModel[]> GetSportsClubModelsBySportAsync(int parkId, SportType sportType)
        {
            var results = await _parkRepository.GetSportsClubsBySportAsync(sportType, parkId);

            if (!results.Any()) return null;

            return _mapper.Map<SportsClubModel[]>(results);
        }
        public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(bool onlyWithClubs = false)
        {
            var parks = _mapper.Map<ICollection<ParkModel>>(await _parkRepository.GetAllParksAsync());

            return from p in parks
                   where !onlyWithClubs || p.SportClubs.Count > 0
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Name,
                       Value = p.ParkId.ToString()
                   };
        }
        public IEnumerable<SelectListItem> GetSportListItems()
        {
            return from i in Enum.GetValues<SportType>().ToArray()
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = i.ToString(),
                       Value = i.ToString()
                   };
        }
        public IEnumerable<SelectListItem> GetSortSelectListItems()
        {
            return from p in typeof(SportsClubModel).GetProperties()
                   where SortingService.IsSortable(p)
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = SortingService.GetDisplayName(p),
                       Value = p.Name
                   };
        }

    }
}
