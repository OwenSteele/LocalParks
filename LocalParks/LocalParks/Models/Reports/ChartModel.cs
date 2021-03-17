using LocalParks.Core.Chart;
using Newtonsoft.Json;

namespace LocalParks.Models.Chart
{
    public class ChartModel
    {
        private readonly string _json;
        private readonly ReportChart _chartData;

        public ChartModel(ReportChart chartData)
        {
            _chartData = chartData;
            _json = JsonConvert.SerializeObject(chartData, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }) ;
        }
        public ReportChart ChartData { get => _chartData; }
        public string Json { get => _json; }
    }
}
