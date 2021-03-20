using System;
using System.Linq;

namespace LocalParks.Core.Chart
{
    public class ChartBuilder
    {
        private readonly ReportChart _chart;
        private readonly ChartType _type;
        private bool _hasDataset;
        private bool _hasLabels;
        public ChartBuilder(ChartType type)
        {

            _chart = new();
            _chart.Data = new();

            _chart.Type = type.ToString();
            _type = type;

            _hasDataset = false;
            _hasLabels = false;
        }
        public ChartBuilder AddDataX(string[] labels)
        {
            if (_type.Equals(ChartType.scatter)) return null;

            if (_hasDataset &&
                (labels.Length != _chart.Data.Datasets[0].Data.Length))
                throw new Exception("Label array length does not match the defined dataset length.");

            _chart.Data.Labels = labels;

            _hasLabels = true;

            return this;
        }

        public ChartBuilder AddDatasetY(decimal[] data, int? dp = null, string label = null, string bgcolor = null, string borderColor = null, int borderWidth = 0, string xAxisId = null, string yAxisId = null)
        {
            if (_type.Equals(ChartType.scatter)) return null;

            if (dp.HasValue)
            {
                var x = (int)dp;
                data = data.Select(d => Math.Round(d, x)).ToArray();
            }

            var dataset = new Dataset
            {
                Data = data
            };
            if (!string.IsNullOrWhiteSpace(label)) dataset.Label = label;
            if (!string.IsNullOrWhiteSpace(yAxisId)) dataset.YAxisId = yAxisId;
            if (!string.IsNullOrWhiteSpace(xAxisId)) dataset.XAxisId = xAxisId;

            dataset.BorderWidth = borderWidth;

            if (!string.IsNullOrWhiteSpace(bgcolor)) dataset.BackgroundColor = new string[] { bgcolor };
            if (!string.IsNullOrWhiteSpace(borderColor)) dataset.BorderColor = new string[] { borderColor };

            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                var existing = _chart.Data.Datasets;

                var datasets = new Dataset[existing.Length + 1];

                datasets.CopyTo(existing, 0);
                datasets[^1] = dataset;

                _chart.Data.Datasets = datasets;

                _hasDataset = true;

                return this;
            }

            _hasDataset = true;

            _chart.Data.Datasets = new Dataset[] { dataset };

            return this;
        }

        //public ChartBuilder AddScatterData(decimal[] xData, decimal[] yData, string label = null, string bgcolor = null, string borderColor = null, int borderWidth = 0, string xAxisId = null, string yAxisId = null)
        //{
        //    if (!_type.Equals(ChartType.scatter)) return null;
        //}

        public ChartBuilder AddBackgroundColors(params string[] colors)
        {
            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                if (colors.Length < _chart.Data.Datasets[^1].Data.Length)
                {
                    var len = colors.Length;

                    Array.Resize(ref colors, _chart.Data.Datasets[^1].Data.Length);

                    var diff = _chart.Data.Datasets[^1].Data.Length - len;

                    int j = 0;
                    for(int i = len; i < diff; i++)
                    {
                        colors[i] = colors[j];
                        j++;
                        if (j >= len) j = 0;
                    }

                    colors[..(diff)].CopyTo(colors, len);
                }

                _chart.Data.Datasets[^1].BackgroundColor = colors;

                return this;
            }

            var dataset = new Dataset
            {
                BackgroundColor = colors
            };

            _chart.Data.Datasets = new Dataset[] { dataset };

            return this;
        }
        public ChartBuilder AddBorderColors(params string[] colors)
        {
            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                if (colors.Length < _chart.Data.Datasets[^1].Data.Length)
                {
                    var len = colors.Length;

                    Array.Resize(ref colors, _chart.Data.Datasets[^1].Data.Length);

                    var diff = _chart.Data.Datasets[^1].Data.Length - len;

                    int j = 0;
                    for (int i = len; i < diff; i++)
                    {
                        colors[i] = colors[j];
                        j++;
                        if (j >= len) j = 0;
                    }

                    colors[..(_chart.Data.Datasets[^1].Data.Length - len)].CopyTo(colors, len);
                }

                _chart.Data.Datasets[^1].BorderColor = colors;

                return this;
            }

            var dataset = new Dataset
            {
                BorderColor = colors
            };

            _chart.Data.Datasets = new Dataset[] { dataset };

            return this;
        }

        public ChartBuilder AddAxesSet(bool xAxis, string id = null, bool? displayOverride = null, string type = null, bool beginAtZero = true)
        {
            if (_chart.Options == null) _chart.Options = new();

            if (_chart.Options.Scales == null) _chart.Options.Scales = new();

            if (xAxis)
            {
                var axis = new XAxes
                {
                    Ticks = new()
                };

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
                var axis = new YAxes
                {
                    Ticks = new()
                };

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

            return this;
        }

        public ReportChart GetChart()
        {
            if (IsComplete())
                return _chart;

            throw new Exception("Chart Instance is not valid - required properties have not been set");
        }

        public ChartBuilder IsLazy(bool value)
        {
                _chart.Lazy = value;

            return this;
        }

        public ChartBuilder IsResponsive(bool value)
        {
                _chart.Responsive = value;

            return this;
        }

        public ChartBuilder SetDuration(int value)
        {
                _chart.Duration = value;

            return this;
        }

        public ChartBuilder SetEasing(string value)
        {
            _chart.Easing = value;

            return this;
        }

        public ChartBuilder SetTitle(string value, bool? displayOverride = null)
        {
                _chart.Title = new();

                _chart.Title.Text = value;

                if (displayOverride != null)
                    _chart.Title.Display = (bool)displayOverride;

                else _chart.Title.Display = true;

            return this;
        }

        private bool IsComplete()
        {
            return _hasDataset && _hasLabels;
        }
    }
}
