using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class PostcodesService : IPostcodesService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public PostcodesService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<PostcodeModel[]> GetAllPostcodesAsync()
        {
            var result = await _parkRepository.GetAllPostcodesAsync();
            return _mapper.Map<PostcodeModel[]>(result);
        }

        public async Task<PostcodeModel> GetPostcodeAsync(string zone)
        {
            var result = await _parkRepository.GetPostcodeByZoneAsync(zone);
            return _mapper.Map<PostcodeModel>(result);
        }
    }
}
