using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ParksService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public ParksService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ParkModel[]> GetAllParkModelsAsync(string sortBy = null)
        {
            var results = _mapper.Map<ParkModel[]>(await _parkRepository.GetAllParksAsync());

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var property = typeof(ParkModel).GetProperty(sortBy);

                if (property == null) return results;

                var sorted = from p in results
                             where property.GetValue(p,null) is IComparable
                             orderby property.GetValue(p, null) descending
                             select p;

                if (!sorted.Any()) return results; // -> ICollection.Count

                return sorted.ToArray();
            }

            return results;
        }
        public async Task<ParkModel[]> GetSearchedParksAsync(
            string searchTerm = null,
            string postcode = null,
            string sortBy = null)
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
        public async Task<PostcodeModel> GetPostcodeAsync(string postcode)
        {
            var result = await _parkRepository.GetPostcodeByZoneAsync(postcode);

            return _mapper.Map<PostcodeModel>(result);
        }

        public async Task<ParkModel> AddParkAsync(ParkModel model)
        {
            var park = _mapper.Map<Park>(model);
            _parkRepository.Add(park);

            if (await _parkRepository.SaveChangesAsync()) return _mapper.Map<ParkModel>(park);

            return null;
        }
        public async Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync()
        {
            var postcodes = _mapper.Map<IEnumerable<PostcodeModel>>(await _parkRepository.GetAllPostcodesAsync());

            return from p in postcodes
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Zone,
                       Value = p.Zone
                   };
        }
        public IEnumerable<SelectListItem> GetSortSelectListItems()
        {
            return from p in typeof(ParkModel).GetProperties()
                   where GetDisplayName(p) != null
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = GetDisplayName(p),
                       Value = p.Name
                   };
        }

        private static string GetDisplayName(PropertyInfo p)
        {
            var attribute = p.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                       .Cast<DisplayNameAttribute>().FirstOrDefault();

            if (attribute == null) return null;

            return attribute.DisplayName;
        }
    }
}
