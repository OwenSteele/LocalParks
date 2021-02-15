using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ParkEventsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public ParkEventsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ParkEventModel[]> GetAllParkEventModelsAsync()
        {
            var results = await _parkRepository.GetAllEventsAsync();

            return _mapper.Map<ParkEventModel[]>(results);
        }
        public async Task<ParkEventModel[]> GetSearchedParkEventModelsAsync(string searchTerm)
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            searchTerm = searchTerm.ToLower();

            var matches = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

            if (!matches.Any()) return null;

            return _mapper.Map<ParkEventModel[]>(matches);
        }
        public async Task<ParkEventModel> GetParkEventModelAsync(int parkId, DateTime date)
        {
            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null) return null;

            return _mapper.Map<ParkEventModel>(result);
        }
        public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync()
        {
            var parks = _mapper.Map<IEnumerable<ParkModel>>(await _parkRepository.GetAllParksAsync());

            return from p in parks
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
    }
}
