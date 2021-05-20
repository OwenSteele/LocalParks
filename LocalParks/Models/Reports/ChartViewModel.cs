using LocalParks.Core.Chart;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LocalParks.Models.Chart
{
    public class ChartViewModel
    {
        private readonly string _json;
        private readonly ReportChart _chartData;

        public ChartViewModel(ReportChart chartData)
        {
            _chartData = chartData;
            _json = JsonConvert.SerializeObject(chartData, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            _json = _json.Replace("scatterData", "data");
        }
        public ReportChart ChartData { get => _chartData; }
        public string Json { get => _json; }
    }
}
