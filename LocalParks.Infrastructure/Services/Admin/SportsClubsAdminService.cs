using AutoMapper;
using LocalParks.Core.Domain;
using LocalParks.Core.Models;
using LocalParks.Data;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services.Admin
{
    public class SportsClubsAdminService : ISportsClubsAdminService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public SportsClubsAdminService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<SportsClubModel> AddNewSportsClubAsync(SportsClubModel model)
        {
            var sportsClub = _mapper.Map<SportsClub>(model);

            sportsClub.Park = await _parkRepository.GetParkByIdAsync(model.ParkId);

            _parkRepository.Add(sportsClub);

            if (await _parkRepository.SaveChangesAsync())
                return _mapper.Map<SportsClubModel>(sportsClub);

            return null;
        }
        public async Task<SportsClubModel> UpdateSportsClubAsync(SportsClubModel model)
        {
            var existing = await _parkRepository.GetSportsClubByIdAsync(model.ClubId);
            if (existing == null) return null;

            _mapper.Map(model, existing);

            if (existing.Park == null || model.ParkId != existing.Park.ParkId)
            {
                existing.Park = await _parkRepository.GetParkByIdAsync(model.ParkId);
            }

            if (await _parkRepository.SaveChangesAsync())
                return _mapper.Map<SportsClubModel>(existing);

            return null;
        }
        public async Task<bool> DeleteSportsClubAsync(SportsClubModel model)
        {
            var sportsClub = _mapper.Map<SportsClub>(model);
            _parkRepository.Delete(sportsClub);

            return await _parkRepository.SaveChangesAsync();
        }

    }
}
