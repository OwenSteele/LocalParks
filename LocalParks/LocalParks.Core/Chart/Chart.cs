namespace LocalParks.Core.Chart
{
    public class ReportChart
    {
        public string Type { get; set; }
        public int? Duration { get; set; }
        public string Easing { get; set; }
        public bool? Responsive { get; set; }
        public Title Title { get; set; }
        public bool? Lazy { get; set; }
        public Data Data { get; set; }
        public Options Options { get; set; }
    }
}
