using LocalParks.Core.Chart;
using LocalParks.Infrastructure.Builders;
using Xunit;

namespace LocalParks.Infrastructure.Tests
{
    public class ChartBuilderTests
    {
        [Fact]
        public void WHEN_Chart_Is_Constructed_THEN_Chart_Cannot_be_retrieved()
        {
            var builder = new ChartBuilder(ChartType.bar);

            var chart = builder.GetChart();

            Assert.Null(chart);
        }
        [Fact]
        public void WHEN_Chart_Is_Constructed_with_required_properties_THEN_Chart_can_be_retrieved()
        {
            var builder = new ChartBuilder(ChartType.bar);

            builder.AddDataX(new string[] { "", "", "", "" });

            builder.AddDatasetY(new decimal[] { 1m, 2m, 3m, 4m });

            var chart = builder.GetChart();

            Assert.NotNull(chart);
            Assert.NotNull(chart.Data);
            Assert.Equal(ChartType.bar.ToString(), chart.Type);
        }
        [Fact]
        public void WHEN_BackgroundColors_is_called_with_null_params_THEN_Random_colors_are_given()
        {
            var builder = new ChartBuilder(ChartType.bar);

            builder.AddDataX(new string[] { "", "", "", "" });
            builder.AddDatasetY(new decimal[] { 1m, 2m, 3m, 4m });

            builder.AddBackgroundColors();

            var chart = builder.GetChart();

            var dataset = chart.Data.Datasets[0];

            Assert.NotNull(chart);
            Assert.Equal(dataset.BackgroundColor.Length, dataset.Data.Length);
            foreach (var color in dataset.BackgroundColor)
            {
                Assert.NotNull(color);
            }
        }
        [Fact]
        public void WHEN_BorderColors_is_called_with_null_params_THEN_Random_colors_are_given()
        {
            var builder = new ChartBuilder(ChartType.bar);

            builder.AddDataX(new string[] { "", "", "", "" });
            builder.AddDatasetY(new decimal[] { 1m, 2m, 3m, 4m });

            builder.AddBorderColors();

            var chart = builder.GetChart();

            var dataset = chart.Data.Datasets[0];

            Assert.NotNull(chart);
            Assert.Equal(dataset.BorderColor.Length, dataset.Data.Length);
            foreach (var color in dataset.BorderColor)
            {
                Assert.NotNull(color);
            }
        }
        [Fact]
        public void WHEN_Title_is_set_without_override_THEN_title_is_displayed()
        {
            var builder = new ChartBuilder(ChartType.bar);

            builder.AddDataX(new string[] { "", "", "", "" });
            builder.AddDatasetY(new decimal[] { 1m, 2m, 3m, 4m });

            builder.SetTitle("test");

            var chart = builder.GetChart();

            Assert.Equal("test", chart.Title.Text);
            Assert.True(chart.Title.Display);
        }
        [Fact]
        public void WHEN_ChartType_is_scatter_THEN_Add_DataSetY_and_AddDataX_return_null()
        {
            var builder = new ChartBuilder(ChartType.scatter);

            var dataX = builder.AddDataX(new string[] { "", "", "", "" });
            var dataY = builder.AddDatasetY(new decimal[] { 1m, 2m, 3m, 4m });
            var scatterData = builder.AddScatterData(new decimal[] { 1m, 2m, 3m, 4m }, new decimal[] { 1m, 2m, 3m, 4m });

            Assert.NotNull(scatterData);
            Assert.Null(dataX);
            Assert.Null(dataY);
        }
        [InlineData(ChartType.bar)]
        [InlineData(ChartType.doughnut)]
        [InlineData(ChartType.line)]
        [InlineData(ChartType.pie)]
        [Theory]
        public void WHEN_ChartType_is_not_scatter_THEN_Add_DataSetY_and_AddDataX_return_ChartBuilder(
            ChartType type)
        {
            var builder = new ChartBuilder(type);

            var dataX = builder.AddDataX(new string[] { "", "", "", "" });
            var dataY = builder.AddDatasetY(new decimal[] { 1m, 2m, 3m, 4m });
            var scatterData = builder.AddScatterData(new decimal[] { 1m, 2m, 3m, 4m }, new decimal[] { 1m, 2m, 3m, 4m });

            Assert.Null(scatterData);
            Assert.NotNull(dataX);
            Assert.NotNull(dataY);
        }
    }
}
