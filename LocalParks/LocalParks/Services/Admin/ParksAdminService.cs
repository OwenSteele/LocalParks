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
    public class ParksAdminService : IParksAdminService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public ParksAdminService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<ParkModel> AddParkAsync(ParkModel model)
        {
            var park = _mapper.Map<Park>(model);

            _parkRepository.Add(park);

            if (await _parkRepository.SaveChangesAsync())
                return _mapper.Map<ParkModel>(park);

            return null;
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
