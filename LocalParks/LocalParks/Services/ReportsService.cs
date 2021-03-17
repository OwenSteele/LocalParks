using AutoMapper;
using LocalParks.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;

        public ReportsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<string[,]>> OrderCountPerDayAsync()
        {
            return null;

        }

        public async Task<string[,]> ParksPerPopulationAsync()
        {
            return null;
        }

        public async Task<string[,]> ParksPerPostcodeAsync()
        {
            return null;
        }

        public async Task<string[,]> ProductsByPercentageAsync()
        {
            return null;
        }

        public async Task<ICollection<string[,]>> ProductsByPercentageByPostcodeAsync()
        {
            return null;
        }

        public async Task<string[,]> SportsByPercentageAsync()
        {
            return null;
        }

        public async Task<string[,]> SupervisorSalaryByTenureAsync()
        {
            var result = await _parkRepository.GetAllSupervisorsAsync();

            var data = new string[2,result.Length];

            for(int i = 0; i < result.Length; i++)
            {
                data[0, i] = result[i].Salary.ToString();
                data[1, i] = ((DateTime.Now - result[i].StartingDate).TotalDays / 365.25).ToString();
            }

            return data;
        }

        public async Task<string[,]> SupervisorSalaryPerMetreSquaredAsync()
        {
            return null;
        }
    }
}
