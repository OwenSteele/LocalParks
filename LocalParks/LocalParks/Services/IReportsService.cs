using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IReportsService
    {
        // parks, sportclubs
        public Task<string[,]> ParksPerPostcodeAsync();
        public Task<string[,]> ParksPerPopulationAsync();
        public Task<string[,]> SportsByPercentageAsync();

        // shop
        public Task<ICollection<string[,]>> OrderCountPerDayAsync();
        public Task<string[,]> ProductsByPercentageAsync();
        public Task<ICollection<string[,]>> ProductsByPercentageByPostcodeAsync();

        // supervisors
        public Task<string[,]> SupervisorSalaryPerMetreSquaredAsync();
        public Task<string[,]> SupervisorSalaryByTenureAsync();

    }
}
