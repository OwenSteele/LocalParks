using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services.Combined;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<ParkModel[]> GetAllModelsAsync(string sortBy = null)
        {
            var results = _mapper.Map<ParkModel[]>(await _parkRepository.GetAllParksAsync());

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(results, sortBy);

            return results;
        }
        public async Task<ParkModel[]> GetSearchedAsync(
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

            var models = _mapper.Map<ParkModel[]>(results);

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(models, sortBy);

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

            if (await _parkRepository.SaveChangesAsync()) 
                return _mapper.Map<ParkModel>(park);

            return null;
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
        public IEnumerable<SelectListItem> GetSortSelectListItems()
        {
            return from p in typeof(ParkModel).GetProperties()
                   where SortingService.IsSortable(p)
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = SortingService.GetDisplayName(p),
                       Value = p.Name
                   };
        }

        public async Task<ParkModel> UpdateParkAsync(ParkModel model)
        {
            var existing = await _parkRepository.GetParkByIdAsync(model.ParkId);
            if (existing == null) return null;

            _mapper.Map(model, existing);

            if (await _parkRepository.SaveChangesAsync()) return _mapper.Map<ParkModel>(existing);

            return null;
        }

        public async Task<bool> DeleteParkAsync(ParkModel model)
        {
            var park = _mapper.Map<Park>(model);
            _parkRepository.Delete(park);

            return await _parkRepository.SaveChangesAsync();
        }
    }
}
