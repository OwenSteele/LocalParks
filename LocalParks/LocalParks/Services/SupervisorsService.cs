using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class SupervisorsService : ISupervisorsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;

        public SupervisorsService(IParkRepository parkRepository, IMapper mapper,
            IEncryptionService encryptionService)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
            _encryptionService = encryptionService;
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
                var parkIdValue = _encryptionService.Decrypt(parkId);
                var park = int.Parse(parkIdValue);

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

        public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(bool onlyWithSupervisors = false)
        {
            var parks = _mapper.Map<ICollection<ParkModel>>(await _parkRepository.GetAllParksAsync());

            return from p in parks
                   where !onlyWithSupervisors || p.Supervisor != null
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Name,
                       Value = _encryptionService.Encrypt(p.ParkId)
                   };
        }

    }
}
