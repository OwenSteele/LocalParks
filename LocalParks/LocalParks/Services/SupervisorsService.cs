using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services.Combined;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class SupervisorsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public SupervisorsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<SupervisorModel[]> GetAllSupervisorModelsAsync(string sortBy = null)
        {
            var results = _mapper.Map<SupervisorModel[]>(await _parkRepository.GetAllSupervisorsAsync());

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(results, sortBy);

            return results;
        }
        public async Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(
            string searchTerm,
            string parkId,
            string sortBy)
        {
            var results = await _parkRepository.GetAllSupervisorsAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                results = results.Where(p =>
                $"{p.FirstName} {p.LastName}".ToLower() == searchTerm |
                $"{p.FirstName} {p.LastName}".ToLower().Contains(searchTerm) |
                $"{p.FirstName} {p.LastName}".ToLower().StartsWith(searchTerm))
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

            var models = _mapper.Map<SupervisorModel[]>(results);

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(models, sortBy);

            return models;
        }
        public async Task<SupervisorModel> GetSupervisorModelAsync(int parkId)
        {
            var result = await _parkRepository.GetSupervisorByParkIdAsync(parkId);

            if (result == null) return null;

            return _mapper.Map<SupervisorModel>(result);
        }
        public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(bool onlyWithEvents = false)
        {
            var parks = _mapper.Map<ICollection<ParkModel>>(await _parkRepository.GetAllParksAsync());

            return from p in parks
                   where !onlyWithEvents || p.Events.Count > 0
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Name,
                       Value = p.ParkId.ToString()
                   };
        }
        public IEnumerable<SelectListItem> GetSortSelectListItems()
        {
            return from p in typeof(SupervisorModel).GetProperties()
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
