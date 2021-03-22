using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services.Admin
{
    public class SupervisorsAdminService : ISupervisorsAdminService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public SupervisorsAdminService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<SupervisorModel> AddNewSupervisorAsync(SupervisorModel model)
        {
            var supervisor = _mapper.Map<Supervisor>(model);

            _parkRepository.Add(supervisor);

            if (await _parkRepository.SaveChangesAsync())
                return _mapper.Map<SupervisorModel>(supervisor);

            return null;
        }
        public async Task<SupervisorModel> UpdateSupervisorAsync(SupervisorModel model)
        {
            var existing = await _parkRepository.GetSupervisorByIdAsync(model.EmployeeId);
            if (existing == null) return null;

            _mapper.Map(model, existing);

            if (await _parkRepository.SaveChangesAsync())
                return _mapper.Map<SupervisorModel>(existing);

            return null;
        }
        public async Task<bool> DeleteSupervisorAsync(SupervisorModel model)
        {
            var supervisor = _mapper.Map<Supervisor>(model);
            _parkRepository.Delete(supervisor);

            return await _parkRepository.SaveChangesAsync();
        }
    }
}
