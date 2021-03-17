namespace LocalParks.Core.Chart
{
    public class Scales
    {
        private XAxes[] _xAxis;
        private YAxes[] _yAxis;

        public Scales(XAxes[] xAxis, YAxes[] yAxis)
        {
            _xAxis = xAxis;
            _yAxis = yAxis;
        }
        public YAxes[] YAxes { get => _yAxis; set => _yAxis = value; }
        public XAxes[] XAxes { get => _xAxis; set => _xAxis = value; }
    }
}