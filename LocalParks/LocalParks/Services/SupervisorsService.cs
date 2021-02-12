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
    public class SupervisorsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public SupervisorsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<SupervisorModel[]> GetAllSupervisorModelsAsync()
        {
            var results = await _parkRepository.GetAllSupervisorsAsync();

            return _mapper.Map<SupervisorModel[]>(results);
        }
        public async Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(string searchTerm)
        {
            var results = await _parkRepository.GetAllSupervisorsAsync();

            searchTerm = searchTerm.ToLower();

            var matches = results.Where(p =>
                string.Join(" ", p.FirstName, p.LastName).ToLower() == searchTerm |
                string.Join(" ", p.FirstName, p.LastName).ToLower().Contains(searchTerm) |
                string.Join(" ", p.FirstName, p.LastName).ToLower().StartsWith(searchTerm))
                    .ToArray();

            if (!matches.Any()) return null;

            return _mapper.Map<SupervisorModel[]>(matches);
        }
        public async Task<SupervisorModel> GetSupervisorModelAsync(int parkId)
        {
            var result = await _parkRepository.GetParkByIdAsync(parkId);

            if (result == null) return null;

            return _mapper.Map<SupervisorModel>(result);
        }
    }
}
