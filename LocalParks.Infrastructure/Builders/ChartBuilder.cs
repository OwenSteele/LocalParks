using LocalParks.Core.Chart;
using System;
using System.Linq;

namespace LocalParks.Infrastructure.Builders
{
    public class ChartBuilder
    {
        private readonly ReportChart _chart;
        private readonly ChartType _type;
        private bool _hasDataset;
        private bool _hasLabels;
        private Random _rnd;
        private string[] _rndColors;

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

        public ChartBuilder AddDatasetY(decimal[] data, int? pointRadii = null, int? dp = null,
            string label = null, string bgcolor = null, string borderColor = null,
            int borderWidth = 1, string xAxisId = null, string yAxisId = null)
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

            if (pointRadii.HasValue) dataset.PointRadius = (int)pointRadii;

            if (!string.IsNullOrWhiteSpace(label)) dataset.Label = label;
            if (!string.IsNullOrWhiteSpace(yAxisId)) dataset.YAxisID = yAxisId;
            if (!string.IsNullOrWhiteSpace(xAxisId)) dataset.XAxisID = xAxisId;

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
        public ChartBuilder AddScatterData(decimal[] xData, decimal[] yData,
            int? pointRadii = null, int? xDp = null, int? yDp = null,
            string label = null, string bgcolor = null, string borderColor = null,
            int borderWidth = 1, string xAxisId = null, string yAxisId = null)
        {
            if (!_type.Equals(ChartType.scatter)) return null;

            if (xDp.HasValue)
            {
                var x = (int)xDp;
                xData = xData.Select(d => Math.Round(d, x)).ToArray();
            }

            if (yDp.HasValue)
            {
                var y = (int)yDp;
                yData = yData.Select(d => Math.Round(d, y)).ToArray();
            }

            if (!xData.Length.Equals(yData.Length))
            {
                var lower = (xData.Length - yData.Length < 0) ? xData.Length : yData.Length;
                Array.Resize(ref xData, lower);
                Array.Resize(ref yData, lower);
            }

            var scatterData = new ScatterData[xData.Length];

            for (int i = 0; i < scatterData.Length; i++)
            {
                scatterData[i] = new ScatterData
                {
                    X = xData[i],
                    Y = yData[i]
                };
            }

            var dataset = new Dataset
            {
                ScatterData = scatterData
            };

            if (pointRadii.HasValue) dataset.PointRadius = (int)pointRadii;

            if (!string.IsNullOrWhiteSpace(label)) dataset.Label = label;
            if (!string.IsNullOrWhiteSpace(yAxisId)) dataset.YAxisID = yAxisId;
            if (!string.IsNullOrWhiteSpace(xAxisId)) dataset.XAxisID = xAxisId;

            dataset.BorderWidth = borderWidth;

            if (!string.IsNullOrWhiteSpace(bgcolor))
            {
                var bgColors = new string[scatterData.Length];
                Array.Fill(bgColors, bgcolor);
                dataset.BackgroundColor = bgColors;
            }
            if (!string.IsNullOrWhiteSpace(borderColor))
            {
                var borderColors = new string[scatterData.Length];
                Array.Fill(borderColors, borderColor);
                dataset.BorderColor = borderColors;
            }

            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                var existing = _chart.Data.Datasets;

                var datasets = new Dataset[existing.Length + 1];

                datasets.CopyTo(existing, 0);
                datasets[^1] = dataset;

                _chart.Data.Datasets = datasets;

                _hasDataset = true;
                _hasLabels = true;

                return this;
            }

            _hasDataset = true;
            _hasLabels = true;

            _chart.Data.Datasets = new Dataset[] { dataset };

            return this;
        }

        public ChartBuilder AddBackgroundColors(params string[] colors)
        {
            int dataLength;

            if (_type.Equals(ChartType.scatter)) dataLength = _chart.Data.Datasets[^1].ScatterData.Length;
            else dataLength = _chart.Data.Datasets[^1].Data.Length;

            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                if (!colors.Any()) _chart.Data.Datasets[^1].BackgroundColor = RandomColors(dataLength, 0.5);

                else _chart.Data.Datasets[^1].BackgroundColor = Resize(colors, dataLength);

                return this;
            }

            var dataset = new Dataset();

            if (!colors.Any()) dataset.BackgroundColor = RandomColors(dataLength, 0.5);
            else dataset.BackgroundColor = colors;

            _chart.Data.Datasets = new Dataset[] { dataset };

            return this;
        }
        public ChartBuilder AddBorderColors(params string[] colors)
        {
            int dataLength;

            if (_type.Equals(ChartType.scatter)) dataLength = _chart.Data.Datasets[^1].ScatterData.Length;
            else dataLength = _chart.Data.Datasets[^1].Data.Length;

            if (_chart.Data.Datasets != null && _chart.Data.Datasets.Length != 0)
            {
                if (!colors.Any()) _chart.Data.Datasets[^1].BorderColor = RandomColors(dataLength);

                else _chart.Data.Datasets[^1].BorderColor = Resize(colors, dataLength);

                return this;
            }

            var dataset = new Dataset();

            if (!colors.Any()) dataset.BorderColor = RandomColors(dataLength);
            else dataset.BorderColor = colors;

            _chart.Data.Datasets = new Dataset[] { dataset };

            return this;
        }
        private static string[] Resize(string[] array, int size)
        {
            if (array.Length >= size) return array;

            var len = array.Length;

            var newArr = new string[size];
            array.CopyTo(newArr, 0);

            int j = 0;
            for (int i = len; i < size; i++)
            {
                newArr[i] = array[j];
                j++;
                if (j >= len) j = 0;
            }

            return newArr;
        }
        private string[] RandomColors(int size, double opacity = 1, int max = 255, int min = 0)
        {
            if (_rndColors != null && _rndColors.Length == size) return _rndColors;

            if (opacity > 1) opacity = 1;
            else if (opacity < 0.1) opacity = 0.1;
            if (_rnd == null) _rnd = new();

            var array = new string[size];

            for (int i = 0; i < size; i++) array[i] =
                    $"rgba({_rnd.Next(min, max)}, {_rnd.Next(min, max)}, {_rnd.Next(min, max)}, {opacity})";

            _rndColors = array;

            return array;
        }

        public ChartBuilder AddXAxesSet(string label, string id = null, bool display = true, string type = null, bool? beginAtZero = null)
        {
            if (_chart.Options == null) _chart.Options = new();

            if (_chart.Options.Scales == null) _chart.Options.Scales = new();

            var axis = new XAxes();

            if (string.IsNullOrWhiteSpace(id)) axis.Id = id;
            if (string.IsNullOrWhiteSpace(type)) axis.Id = id;

            axis.Display = display;

            if (beginAtZero.HasValue)
            {
                axis.Ticks = new();
                axis.Ticks.BeginAtZero = (bool)beginAtZero;
            }

            axis.ScaleLabel = new ScaleLabel
            {
                Display = display,
                LabelString = label
            };

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

            if (_chart.Data.Datasets != null)
            {
                _chart.Data.Datasets[^1].XAxisID ??= id;
            }

            return this;
        }
        public ChartBuilder AddYAxesSet(string label, string id = null, bool display = true, string type = null, bool? beginAtZero = null)
        {
            if (_chart.Options == null) _chart.Options = new();

            if (_chart.Options.Scales == null) _chart.Options.Scales = new();

            var axis = new YAxes();

            if (string.IsNullOrWhiteSpace(id)) axis.Id = id;
            if (string.IsNullOrWhiteSpace(type)) axis.Id = id;

            axis.Display = display;

            if (beginAtZero.HasValue)
            {
                axis.Ticks = new();
                axis.Ticks.BeginAtZero = (bool)beginAtZero;
            }

            axis.ScaleLabel = new ScaleLabel
            {
                Display = display,
                LabelString = label
            };

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

            if (_chart.Data.Datasets != null)
            {
                _chart.Data.Datasets[^1].YAxisID ??= id;
            }

            return this;
        }

        public ReportChart GetChart()
        {
            if (!IsComplete()) return null;

            return _chart;
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

        public ChartBuilder SetMaintainAspectRatio(bool value = false, bool responsive = true)
        {
            if (_chart.Options == null) _chart.Options = new();

            _chart.Options.MaintainAspectRatio = value;
            _chart.Options.Responsive = responsive;

            return this;
        }

        private bool IsComplete()
        {
            return _hasDataset && _hasLabels;
        }
    }
}
