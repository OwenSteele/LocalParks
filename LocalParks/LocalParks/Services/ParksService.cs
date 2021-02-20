﻿using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using System.Linq;
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
        public async Task<ParkModel[]> GetAllParkModelsAsync()
        {
            var results = await _parkRepository.GetAllParksAsync();

            return _mapper.Map<ParkModel[]>(results);
        }
        public async Task<ParkModel[]> GetSearchedParksAsync(string searchTerm)
        {
            var results = await _parkRepository.GetAllParksAsync();

            searchTerm = searchTerm.ToLower();

            var matches = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

            if (!matches.Any()) return null;

            return _mapper.Map<ParkModel[]>(matches);
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

            if(await _parkRepository.SaveChangesAsync()) return _mapper.Map<ParkModel>(park);

            return null;
        }
    }
}
