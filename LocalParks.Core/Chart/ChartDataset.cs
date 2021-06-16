namespace LocalParks.Core.Chart
{
    public class Dataset
    {
        public string Label { get; set; }
        public decimal[] Data { get; set; }
        public ScatterData[] ScatterData { get; set; }
        public string[] BackgroundColor { get; set; }
        public string[] BorderColor { get; set; }
        public int BorderWidth { get; set; }
        public string YAxisID { get; set; }
        public string XAxisID { get; set; }
        public int PointRadius { get; set; }
    }
}