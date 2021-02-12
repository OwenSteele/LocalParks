using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<SportsClubModel[]> GetAllSportsClubModelsAsync()
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            return _mapper.Map<SportsClubModel[]>(results);
        }
        public async Task<SportsClubModel[]> GetSearchedSportsClubModelsAsync(string searchTerm)
        {
            var results = await _parkRepository.GetAllSportsClubsAsync();

            searchTerm = searchTerm.ToLower();

            var matches = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

            if (!matches.Any()) return null;

            return _mapper.Map<SportsClubModel[]>(matches);
        }
        public async Task<SportsClubModel> GetSportsClubModelAsync(int clubId)
        {
            var result = await _parkRepository.GetSportsClubByIdAsync(clubId);

            if (result == null) return null;

            return _mapper.Map<SportsClubModel>(result);
        }
    }
}
