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
    public class ParkEventsService : IParkEventsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public ParkEventsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ParkEventModel[]> GetAllParkEventModelsAsync(int? parkId = null, string sortBy = null)
        {
            var results = parkId == null ?
                    _mapper.Map<ParkEventModel[]>(await _parkRepository.GetAllEventsAsync()) :
                    _mapper.Map<ParkEventModel[]>(await _parkRepository.GetEventsByParkIdAsync((int)parkId));

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(results, sortBy);

            return results;
        }
        public async Task<ParkEventModel[]> GetSearchedParkEventModelsAsync(
            string searchTerm = null,
            string parkId = null,
            DateTime? date = null,
            string sortBy = null)
        {
            var results = await _parkRepository.GetAllEventsAsync();

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
            if (date != null)
            {
                var dateValue = (DateTime)date;

                results = results.Where(p =>
                p.Date == dateValue.Date)
                    .ToArray();

                if (!results.Any()) return null;
            }

            var models = _mapper.Map<ParkEventModel[]>(results);

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(models, sortBy);

            return models;
        }
        public async Task<ParkEventModel> GetParkEventModelAsync(int parkId, DateTime date)
        {
            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null) return null;

            return _mapper.Map<ParkEventModel>(result);
        }
        public async Task<ParkEventModel> GetParkEventModelByIdAsync(int eventId, int? parkId = null)
        {
            var result = await _parkRepository.GetEventByIdAsync(eventId);

            if (result == null) return null;

            if (parkId != null && result.Park.ParkId != parkId) return null;

            return _mapper.Map<ParkEventModel>(result);
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
        public async Task<ParkModel> GetParkAsync(int parkId)
        {
            var result = await _parkRepository.GetParkByIdAsync(parkId);

            if (result == null) return null;

            return _mapper.Map<ParkModel>(result);
        }

        public async Task<ParkEventModel> CreateNewEventAsync(ParkEventModel newEvent, ParkModel park)
        {
            var result = _mapper.Map<ParkEvent>(newEvent);

            result.Park = _mapper.Map<Park>(park);

            _parkRepository.Add(result);

            if (!await _parkRepository.SaveChangesAsync()) return null;

            var parkEvent = await _parkRepository.GetEventByParkIdByDateAsync(newEvent.ParkId, newEvent.Date);

            return _mapper.Map<ParkEventModel>(parkEvent);
        }
        public async Task<bool> RemoveEventAsync(ParkEventModel parkEventModel)
        {
            var parkEvent = _mapper.Map<ParkEvent>(parkEventModel);

            _parkRepository.Delete(parkEvent);

            return await _parkRepository.SaveChangesAsync();
        }
        public IEnumerable<SelectListItem> GetSortSelectListItems()
        {
            return from p in typeof(ParkEventModel).GetProperties()
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
