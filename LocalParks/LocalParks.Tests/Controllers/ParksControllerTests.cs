using AutoMapper;
using LocalParks.Controllers;
using LocalParks.Models;
using LocalParks.Services;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LocalParks.Tests.Controllers
{
    public class ParksControllerTests
    {
        private readonly Mock<ILogger<ParksController>> _mockLogger;

        private readonly Mock<IParksService> _mockService;
        private readonly Mock<ISortingService> _mockSortService;
        private readonly Mock<ClaimsPrincipal> _mockUser;
        private readonly TempDataDictionary _tempData;
        private readonly ParkModel[] _parks;

        public ParksControllerTests()
        {
            var myProfile = new ParkProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            _parks = GetTestParks();

            var httpContext = new DefaultHttpContext();
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _mockLogger = new Mock<ILogger<ParksController>>();
            _mockService = new Mock<IParksService>();
            _mockSortService = new Mock<ISortingService>();

            _mockUser = new Mock<ClaimsPrincipal>();
            _mockUser.Setup(s => s.AddIdentity(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "mock"),
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    }, "mock")));

        }
        private ParksController ArrangeController() => new ParksController(
            _mockLogger.Object,
                _mockService.Object)
        {
            TempData = _tempData,
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser.Object }
            }
        };

        private void ArrangeServiceAllModels()
        {
            _mockService.Setup(s => s.GetAllModelsAsync()).ReturnsAsync(_parks);
        }

        [Fact]
        public async Task WHEN_Index_action_is_called_THEN_Index_view_is_returned_with_ParkModel()
        {
            ArrangeServiceAllModels();

            var controller = ArrangeController();

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.True(model.Any());
        }
        [InlineData(true, 0, null)]
        [InlineData(true, 1, "one")]
        [InlineData(true, 1, "ktw")]
        [InlineData(false, 2, "xxx")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_searchTerm_THEN_Index_view_is_returned_only_with_matching_events(
            bool isAny, int missingParks, string searchterm)
        {
            _mockService.Setup(s => s.GetSearchedAsync(searchterm, null, false))
                .ReturnsAsync(_parks.Where(e =>
               string.IsNullOrEmpty(searchterm) ||
               e.Name.ToLower().Contains(searchterm.ToLower())).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, searchterm, null, null, null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(isAny, model.Any());
            Assert.Equal(_parks.Length - missingParks, model.Count());
        }
        [InlineData(true, 0, null)]
        [InlineData(true, 1, "lp1")]
        [InlineData(true, 1, "lP2")]
        [InlineData(false, 2, "xxx")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_postcode_THEN_Index_view_is_returned_only_with_matching_events(
            bool isAny, int missingParks, string postcode)
        {
            _mockService.Setup(s => s.GetSearchedAsync(null, postcode, false))
                .ReturnsAsync(_parks.Where(e =>
               string.IsNullOrEmpty(postcode) ||
               e.PostcodeZone.ToLower().Equals(postcode.ToLower())).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, null, postcode, null, null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(isAny, model.Any());
            Assert.Equal(_parks.Length - missingParks, model.Count());
        }
        [Fact]
        public async Task WHEN_Filter_action_is_called_with_Time_THEN_Index_view_is_returned_only_with_open_parks()
        {
            ArrangeServiceAllModels();

            var controller = ArrangeController();

            var now = DateTime.Now;

            var expectedOpen = _parks.Where(p => p.OpeningTime <= now && p.ClosingTime >= now).Count();

            var result = await controller.Filter(_mockSortService.Object, null, null, "yes");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(expectedOpen, model.Count());
        }

        [InlineData(null, null, null)]
        [InlineData("One", null, "One")]
        [InlineData("kTw", null, "kTw")]
        [InlineData("xxx", null, "xxx")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_searchTerm_THEN_Index_view_TempData_containing_searchTerm(
            string tdFilter, string tdMatches, string searchterm)
        {
            _mockService.Setup(s => s.GetSearchedAsync(searchterm, null, false))
                .ReturnsAsync(_parks.Where(e =>
               string.IsNullOrEmpty(searchterm) ||
               e.Name.ToLower().Contains(searchterm.ToLower())).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(tdFilter, viewResult.TempData["Filter"]);
            Assert.Equal(tdMatches, viewResult.TempData["Matches"]);
        }

        [InlineData(0, 5001)]
        [InlineData(1, 5002)]
        [Theory]
        public async Task WHEN_details_action_is_called_with_parkId_THEN_details_view_is_returned_containing_matching_ParkModel(
            int parkPos, int parkId)
        {
            _mockService.Setup(s => s.GetParkAsync(parkId))
                .ReturnsAsync(_parks.Where(e => e.ParkId == parkId).FirstOrDefault());

            var controller = ArrangeController();

            var result = await controller.Details(parkId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkModel>(
                viewResult.ViewData.Model);

            Assert.Null(viewResult.ViewName);
            Assert.Equal(model.ParkId, _parks[parkPos].ParkId);
        }

        internal static ParkModel[] GetTestParks()
        {
            return new ParkModel[]
            {
                new ParkModel {
                    ParkId = 5001,
                PostcodeZone = "LP1" ,
                    Name = "TestingParkOne",
                    SizeInMetresSquared = 20000,
                    Longitude = 12.345m,
                    Latitude = 5.4321m,
                    OpeningTime = new DateTime (2000,1,1,7,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,19,0,0 ),
                    Supervisor = new SupervisorModel{ EmployeeId = 5101, ParkId = 5001}
                },
                new ParkModel {
                    ParkId = 5002,
                    PostcodeZone =  "LP2",
                    Name = "TestingParkTwo",
                    SizeInMetresSquared = 1000,
                    Longitude = 1.2345m,
                    Latitude = 54.321m,
                    OpeningTime = new DateTime (2000,1,1,6,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,20,0,0 ),
                    Supervisor = new SupervisorModel{ EmployeeId = 5102, ParkId = 5002}
                }
            };
        }
    }
}
