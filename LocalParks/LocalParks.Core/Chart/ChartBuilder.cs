using System;

namespace LocalParks.Core.Chart
{
    public class ChartBuilder
    {
        private readonly ReportChart _chart;
        private bool _hasDataset;
        private bool _hasLabels;
        public ChartBuilder()
        {
            _chart = new();
            _chart.Data = new();

            _hasDataset = false;
            _hasLabels = false;
        }
        public void AddDataLabels(string[] labels)
        {
            if (_hasDataset &&
                (labels.Length != _chart.Data.Datasets[0].Data.Length))
                throw new Exception("Label array length does not match the defined dataset length.");

            _chart.Data.Labels = labels;

            _hasLabels = true;
        }

        public void AddDataset(int[] data, string label = null, string[] bgcolors = null, string[] borderColors = null, int borderWidth = 0, string xAxisId = null, string yAxisId = null)
        {
            var dataset = new Dataset
            {
                Data = data
            };
            if (!string.IsNullOrWhiteSpace(label)) dataset.Label = label;
            if (!string.IsNullOrWhiteSpace(yAxisId)) dataset.YAxisId = yAxisId;
            if (!string.IsNullOrWhiteSpace(xAxisId)) dataset.XAxisId = xAxisId;

            dataset.BorderWidth = borderWidth;

            if (bgcolors != null && bgcolors.Length != 0) dataset.BackgroundColor = bgcolors;
            if (borderColors != null && borderColors.Length != 0) dataset.BorderColor = borderColors;


            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                var existing = _chart.Data.Datasets;

                var datasets = new Dataset[existing.Length + 1];

                datasets.CopyTo(existing, 0);
                datasets[^1] = dataset;

                _chart.Data.Datasets = datasets;

                _hasDataset = true;

                return;
            }

            _hasDataset = true;

            _chart.Data.Datasets = new Dataset[] { dataset };
        }

        public void AddAxesSet(bool xAxis, string id = null, bool? displayOverride = null, string type = null, bool beginAtZero = true)
        {
            if (_chart.Options == null) _chart.Options = new();

            if (_chart.Options.Scales == null) _chart.Options.Scales = new();

            if (xAxis)
            {
                var axis = new XAxes();

                axis.Ticks = new();

                if (string.IsNullOrWhiteSpace(id)) axis.Id = id;
                if (displayOverride.HasValue) axis.Display = (bool)displayOverride;
                if (string.IsNullOrWhiteSpace(type)) axis.Id = id;

                axis.Ticks.BeginAtZero = beginAtZero;

                if (_chart.Options.Scales.XAxes == null || _chart.Options.Scales.XAxes.Length == 0)
                {
                    _chart.Options.Scales.XAxes = new XAxes[] { axis };
                }
                else
                {
                    var existing = _chart.Options.Scales.XAxes;

                    var axes = new XAxes[existing.Length + 1];

                    axes.CopyTo(existing, 0);
                    axes[^1] = axis;

                    _chart.Options.Scales.XAxes = axes;
                }
            }
            else
            {
                var axis = new YAxes();

                axis.Ticks = new();

                if (string.IsNullOrWhiteSpace(id)) axis.Id = id;
                if (displayOverride.HasValue) axis.Display = (bool)displayOverride;
                if (string.IsNullOrWhiteSpace(type)) axis.Id = id;

                axis.Ticks.BeginAtZero = beginAtZero;

                if (_chart.Options.Scales.YAxes == null || _chart.Options.Scales.YAxes.Length == 0)
                {
                    _chart.Options.Scales.YAxes = new YAxes[] { axis };
                }
                else
                {
                    var existing = _chart.Options.Scales.YAxes;

                    var axes = new YAxes[existing.Length + 1];

                    axes.CopyTo(existing, 0);
                    axes[^1] = axis;

                    _chart.Options.Scales.YAxes = axes;
                }
            }
        }

        public ReportChart GetChart()
        {
            if (ValidForReturn())
                return _chart;

            throw new Exception("Chart Instance is not valid - required properties have not been set");
        }

        public void IsLazy(bool? value = null)
        {
            if (value.HasValue)
                _chart.Lazy = value;
        }

        public void IsResponsive(bool? value = null)
        {
            if (value.HasValue)
                _chart.Responsive = value;
        }

        public void SetDuration(int? value = null)
        {
            if (value.HasValue)
                _chart.Duration = value;
        }

        public void SetEasing(string value = null)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _chart.Easing = value;
        }

        public void SetTitle(string value = null, bool? displayOverride = null)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _chart.Title = new();

                _chart.Title.Text = value;

                if (displayOverride != null)
                    _chart.Title.Display = (bool)displayOverride;

                else _chart.Title.Display = true;
            }
        }

        public void SetType(string value = null)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _chart.Type = value;
        }

        private bool ValidForReturn()
        {
            return _hasDataset && _hasLabels;
        }
    }
}
