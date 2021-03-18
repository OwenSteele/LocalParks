namespace LocalParks.Core.Chart
{
    public class Dataset
    {
        public string Label { get; set; }
        public int[] Data { get; set; }
        public string[] BackgroundColor { get; set; }
        public string[] BorderColor { get; set; }
        public int BorderWidth { get; set; }
        public string YAxisId { get; set; }
        public string XAxisId { get; set; }
    }
}