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
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IParkRepository _parkRepository;

        public UserService(IMapper mapper, IParkRepository parkRepository)
        {
            _mapper = mapper;
            _parkRepository = parkRepository;
        }


        public async Task<LocalParksUserModel> GetUserAsync(string name)
        {
            var user = await _parkRepository.GetLocalParksUserByUsernameAsync(name);

            return _mapper.Map<LocalParksUserModel>(user);
        }
        public async Task<LocalParksUserModel> GetUserByEmailAsync(string email)
        {
            var user = await _parkRepository.GetLocalParksUserByEmailAsync(email);

            return _mapper.Map<LocalParksUserModel>(user);
        }
    }
}
