using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ParksService : IParksService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public ParksService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ParkModel[]> GetAllModelsAsync()
        {
            var results = _mapper.Map<ParkModel[]>(await _parkRepository.GetAllParksAsync());

            return results;
        }
        public async Task<ParkModel[]> GetSearchedAsync(
            string searchTerm = null,
            string postcode = null,
            bool openOnly = false)
        {
            var results = await _parkRepository.GetAllParksAsync();

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

            if (!string.IsNullOrWhiteSpace(postcode))
            {
                postcode = postcode.ToLower();

                results = results.Where(p =>
                p.PostcodeZone.ToLower() == postcode |
                p.PostcodeZone.ToLower().Contains(postcode) |
                p.PostcodeZone.ToLower().StartsWith(postcode))
                    .ToArray();

                if (!results.Any()) return null;
            }

            if (openOnly)
            {
                var now = DateTime.Now.TimeOfDay;

                results = results.Where(p =>
                now > p.OpeningTime.TimeOfDay
                && now < p.ClosingTime.TimeOfDay)
                    .ToArray();
            }

            return _mapper.Map<ParkModel[]>(results);
        }
        public async Task<ParkModel> GetParkAsync(int parkId)
        {
            var result = await _parkRepository.GetParkByIdAsync(parkId);

            if (result == null) return null;

            return _mapper.Map<ParkModel>(result);
        }
        public async Task<ParkModel> GetParkAsync(string parkName)
        {
            var result = await _parkRepository.GetParkByNameAsync(parkName);

            if (result == null) return null;

            return _mapper.Map<ParkModel>(result);
        }
        public async Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync()
        {
            var postcodes = _mapper.Map<IEnumerable<PostcodeModel>>(await _parkRepository.GetAllPostcodesAsync());

            return from p in postcodes
                   where p.Parks.Count > 0
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Zone,
                       Value = p.Zone
                   };
        }
    }
}
