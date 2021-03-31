using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services.ViewComponents
{
    public delegate IChartService ChartServiceResolver(ChartServiceType serviceType);
}
