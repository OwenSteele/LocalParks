namespace LocalParks.Core.Chart
{
    public class ReportChart
    {
        private string _type;
        private int? _duration;
        private string _easing;
        private bool? _responsive;
        private Title _title;
        private bool? _lazy;
        private Data _data;
        private Options _options;

        public ReportChart(
            Data data,
            XAxes[] xAxis = null,
            YAxes[] yAxis = null,
            string type = "bar",
            int? duration = null,
            bool? lazy = null,
            bool? responsive = null,
            Title title = null,
            string easing = null)
        {
            _type = type;
            _duration = duration;
            _easing = easing;
            _responsive = responsive;
            _title = title;
            _lazy = lazy;

            _data = data;

            _options = new Options { Scales = new Scales(xAxis, yAxis) };
        }

        public string Type { get => _type; set => _type = value; }
        public int? Duration { get => _duration; set => _duration = value; }
        public string Easing { get => _easing; set => _easing = value; }
        public bool? Responsive { get => _responsive; set => _responsive = value; }
        public Title Title { get => _title; set => _title = value; }
        public bool? Lazy { get => _lazy; set => _lazy = value; }
        public Data Data { get => _data; set => _data = value; }
        public Options Options { get => _options; set => _options = value; }
    }
}
