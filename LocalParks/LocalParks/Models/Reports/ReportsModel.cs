using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Models.Reports
{
    public class ReportsModel
    {
        public string[,] ParksPerPostcode { get; set; }
        public string[,] ParksPerPopulation { get; set; }
        public string[,] SportsByPercentag { get; set; }
        public string[,] SupervisorSalaryPerMetreSquared { get; set; }
        public string[,] SupervisorSalaryByTenure { get; set; }
    }
}
