using AutoMapper;
using LocalParks.Core.Contracts;
using LocalParks.Core.Domain;
using LocalParks.Core.Models;
using LocalParks.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services
{
    public class SupervisorsService : ISupervisorsService
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
            var results = _mapper.Map<SupervisorModel[]>(await _parkRepository.GetAllSupervisorsAsync());

            return results;
        }
        public async Task<SupervisorModel[]> GetSearchedSupervisorModelsAsync(
            string searchTerm = null,
            string parkId = null)
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

            return _mapper.Map<SupervisorModel[]>(results);
        }
        public async Task<SupervisorModel> GetSupervisorModelAsync(int Id, bool UseParkId = true)
        {
            Supervisor result;

            if (UseParkId) result = await _parkRepository.GetSupervisorByParkIdAsync(Id);
            else result = await _parkRepository.GetSupervisorByIdAsync(Id);

            if (result == null) return null;

            return _mapper.Map<SupervisorModel>(result);
        }
        public async Task<bool> CheckParkExistsAsync(int parkId, bool IfHasSupervisorReturnFalse = false)
        {
            var result = await _parkRepository.GetParkByIdAsync(parkId);

            if (IfHasSupervisorReturnFalse && result != null)
            {
                return result.Supervisor == null;
            }

            return result == null;
        }
    }
}
