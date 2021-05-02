using AutoMapper;
using LocalParks.Controllers;
using LocalParks.Core.Contracts;
using LocalParks.Core.Models;
using LocalParks.Infrastructure.Models;
using LocalParks.Services.View;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ParkEventsControllerTests
    {
        private readonly Mock<ILogger<ParkEventsController>> _mockLogger;
        private readonly Mock<IAuthenticationService> _mockAuthService;
        private readonly Mock<IParkEventsService> _mockService;
        private readonly Mock<ISortingService> _mockSortService;
        private readonly Mock<ISelectListService> _mockSelectListService;
        private readonly Mock<ClaimsPrincipal> _mockUser;
        private readonly TempDataDictionary _tempData;
        private readonly ParkEventModel[] _events;
        private readonly ParkModel[] _parks;

        public ParkEventsControllerTests()
        {
            var myProfile = new ParkProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            _events = GetTestParkEvents();
            _parks = GetTestParks();

            var httpContext = new DefaultHttpContext();
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            _mockLogger = new Mock<ILogger<ParkEventsController>>();
            _mockAuthService = new Mock<IAuthenticationService>();
            _mockService = new Mock<IParkEventsService>();
            _mockSortService = new Mock<ISortingService>();

            _mockUser = new Mock<ClaimsPrincipal>();
            _mockUser.Setup(s => s.AddIdentity(new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "mock"),
                        new Claim(ClaimTypes.NameIdentifier, "1")
                    }, "mock")));

            _mockAuthService.Setup(s => s.IsSignedInAsync(_mockUser.Object))
                .ReturnsAsync(true);

            _mockSelectListService.Setup(s => s.GetParkSelectListItemsAsync<ParkEventModel>(true))
                .ReturnsAsync(_parks.Where(p => p.Events.Any())
                .Select(p => new SelectListItem
                {
                    Selected = false,
                    Text = p.Name,
                    Value = p.ParkId.ToString()
                }));
        }

        private ParkEventsController ArrangeController() => new ParkEventsController(
            _mockLogger.Object,
                _mockService.Object,
                _mockAuthService.Object)
        {
            TempData = _tempData,
            ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext() { User = _mockUser.Object }
            }
        };


        private void ArrangeServiceModelById(int eventId)
        {
            _mockService.Setup(s => s.GetParkEventModelByIdAsync(eventId, null))
                .ReturnsAsync(_events.Where(p => p.EventId == eventId).First());
        }
        private void ArrangeServicePark(int parkId, int pos)
        {
            _mockService.Setup(s => s.GetParkAsync(parkId))
                .ReturnsAsync(GetTestParks()[pos]);
        }
        private void ArrangeRole(bool conditionsMet)
        {
            _mockAuthService.Setup(s => s.HasRequiredRoleAsync("", ""))
                .ReturnsAsync(conditionsMet);
        }

        [Fact]
        public async Task WHEN_Index_action_is_called_THEN_Index_view_is_returned_with_ParkEventModel()
        {
            _mockService.Setup(s => s.GetAllParkEventModelsAsync())
                .ReturnsAsync(_events);

            var controller = ArrangeController();

            var result = await controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.True(model.Any());
        }

        [InlineData(true, 0, null)]
        [InlineData(true, 1, "a BIg")]
        [InlineData(true, 1, "another")]
        [InlineData(false, 2, "xxx")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_searchTerm_THEN_Index_view_is_returned_only_with_matching_events(
            bool isAny, int missingEvents, string searchterm)
        {
            _mockService.Setup(s => s.GetSearchedParkEventModelsAsync(searchterm, null, null))
                .ReturnsAsync(_events.Where(e =>
               string.IsNullOrEmpty(searchterm) ||
               e.Name.ToLower().Contains(searchterm.ToLower())).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(isAny, model.Any());
            Assert.Equal(_events.Length - missingEvents, model.Count());
        }
        [InlineData(true, 0, null)]
        [InlineData(true, 1, "8001")]
        [InlineData(true, 1, "8002")]
        [InlineData(false, 2, "9999")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_parkFilter_THEN_Index_view_is_returned_only_with_matching_events(
           bool isAny, int missingEvents, string parkFilter)
        {
            _mockService.Setup(s => s.GetSearchedParkEventModelsAsync(null, parkFilter, null))
                .ReturnsAsync(_events.Where(e =>
                string.IsNullOrEmpty(parkFilter) ||
                 e.ParkId == int.Parse(parkFilter)).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, parkFilter: parkFilter);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(isAny, model.Any());
            Assert.Equal(_events.Length - missingEvents, model.Count());
        }
        [InlineData(true, 0, null)]
        [InlineData(true, 1, "2200-1-1")]
        [InlineData(true, 1, "2200-1-2")]
        [InlineData(false, 2, "2200-1-3")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_date_THEN_Index_view_is_returned_only_with_matching_events(
           bool isAny, int missingEvents, string dateNull)
        {
            DateTime? date = null;
            if (DateTime.TryParse(dateNull, out DateTime attempt))
            {
                date = attempt;
            }

            _mockService.Setup(s => s.GetSearchedParkEventModelsAsync(null, null, date))
                .ReturnsAsync(_events.Where(e => !date.HasValue || e.Date == (DateTime)date).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, date: date);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(isAny, model.Any());
            Assert.Equal(_events.Length - missingEvents, model.Count());
        }

        [InlineData(null, null, null)]
        [InlineData("a BIg", null, "a BIg")]
        [InlineData("anOTHer", null, "anOTHer")]
        [InlineData("xxx", null, "xxx")]
        [Theory]
        public async Task WHEN_Filter_action_is_called_with_searchTerm_THEN_Index_view_TempData_containing_searchTerm(
            string tdFilter, string tdMatches, string searchterm)
        {
            _mockService.Setup(s => s.GetSearchedParkEventModelsAsync(searchterm, null, null))
                .ReturnsAsync(_events.Where(e =>
                string.IsNullOrEmpty(searchterm) ||
                 e.Name.ToLower().Contains(searchterm.ToLower())).ToArray());

            var controller = ArrangeController();

            var result = await controller.Filter(_mockSortService.Object, searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.Equal("Index", viewResult.ViewName);
            Assert.Equal(tdFilter, viewResult.TempData["Filter"]);
            Assert.Equal(tdMatches, viewResult.TempData["Matches"]);
        }

        [InlineData(0, 7001)]
        [InlineData(1, 7002)]
        [Theory]
        public async Task WHEN_details_action_is_called_with_eventId_THEN_details_view_is_returned_containing_matching_ParkEventModel(
            int parkPos, int eventId)
        {
            ArrangeRole(true);
            ArrangeServiceModelById(eventId);

            var controller = ArrangeController();

            var result = await controller.Details(eventId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.Null(viewResult.ViewName);
            Assert.Equal(_events[parkPos].ParkId, model.ParkId);
        }

        [Fact]
        public async Task WHEN_Edit_action_is_called_without_eventId_THEN_edit_view_without_a_model_is_returned()
        {
            ArrangeRole(true);

            var controller = ArrangeController();

            var result = await controller.Edit(0);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Null(model.Name);
        }

        [InlineData(7001)]
        [InlineData(7002)]
        [Theory]
        public async Task WHEN_Edit_action_is_called_with_eventId_THEN_edit_view_containing_matching_ParkEventModel_is_returned(
            int eventId)
        {
            ArrangeRole(true);
            ArrangeServiceModelById(eventId);

            var controller = ArrangeController();

            var result = await controller.Edit(eventId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.NotNull(model.Name);
            Assert.Equal(eventId, model.EventId);
        }

        [Fact]
        public async Task WHEN_Edit_action_is_called_with_nonexistant_parkID_THEN_edit_view_without_a_model_is_returned()
        {
            ArrangeRole(true);
            ArrangeServicePark(5001, 0);

            var controller = ArrangeController();

            controller.Event = new ParkEventModel { ParkId = 11111 };

            var result = await controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Null(model.Name);
        }

        [Fact]
        public async Task WHEN_Edit_action_is_called_with_date_that_is_taken_THEN_edit_view_is_returned_with_ViewData_Error()
        {
            ArrangeRole(true);
            ArrangeServicePark(4001, 0);

            var controller = ArrangeController();

            controller.Event = new ParkEventModel
            {
                ParkId = 4001,
                ParkName = "TestingParkOne",
                EventId = 9001,
                Name = "NewParkEventTest",
                Date = new DateTime(2200, 12, 12)
            };

            var result = await controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Equal(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.NotNull(model.Name);
        }
        [Fact]
        public async Task WHEN_Edit_action_is_called_with_model_error_THEN_edit_view_is_returned_with_an_invalid_model()
        {
            ArrangeServicePark(5001, 0);

            var controller = ArrangeController();

            controller.Event = new();

            controller.ModelState.AddModelError("Name", "Required");

            var result = await controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Null(model.Name);
        }

        [InlineData(7001)]
        [InlineData(7002)]
        [Theory]
        public async Task WHEN_Delete_action_is_called_with_eventId_THEN_Details_view_is_returned_containing_match_ParkEventModel(int eventId)
        {

            ArrangeServiceModelById(eventId);

            var controller = ArrangeController();

            var result = await controller.Delete(eventId);

            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ParkEvents", actionResult.ControllerName);
            Assert.Equal("Details", actionResult.ActionName);
        }

        [Fact]
        public async Task WHEN_Delete_action_is_called_without_confirmation_THEN_Delete_view_is_returned()
        {
            var eventId = 7001;

            ArrangeServiceModelById(eventId);

            var controller = ArrangeController();

            var result = await controller.Delete(eventId, false);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(model.Name);
            Assert.Equal(eventId, model.EventId);
        }

        private static ParkEventModel[] GetTestParkEvents()
        {
            return new ParkEventModel[]
            {
                new ParkEventModel
                {
                    EventId = 7001,
                    Name = "A big event",
                    ParkId = 8001,
                    ParkName = "TestingParkOne for Event",
                    Date = new DateTime(2200,1,1),
                    Description = "djfglshdflgkjsdfhglkjsdhfg",
                    OrganiserEmail = "fdg@sdf.coum",
                    OrganiserPhoneNumber = "07123456789",
                    OrganiserFirstName = "Joe",
                    OrganiserLastName = "Bloggs"
                },
                new ParkEventModel
                {
                    EventId = 7002,
                    Name = "Another big event",
                    ParkId = 8002,
                    ParkName = "TestingParkTwo for Event",
                    Date = new DateTime(2200,1,2),
                    Description = "mjkllkjhkjh",
                    OrganiserEmail = "uy@uy.com",
                    OrganiserPhoneNumber = "07123456000",
                    OrganiserFirstName = "Joan",
                    OrganiserLastName = "Blogsmith"
                }
            };
        }
        private static ParkModel[] GetTestParks()
        {
            return new ParkModel[]
            {
                new ParkModel {
                    ParkId = 4001,
                PostcodeZone = "LP1" ,
                    Name = "TestingParkOne",
                    SizeInMetresSquared = 20000,
                    Longitude = 12.345m,
                    Latitude = 5.4321m,
                    OpeningTime = new DateTime (2000,1,1,7,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,19,0,0 ),
                    Supervisor = new SupervisorModel{ EmployeeId = 5101, ParkId = 5001},
                    Events = new ParkEventModel[]
                    {
                        new ParkEventModel
                {
                    EventId = 14001,
                    Name = "Another big event",
                    Date = new DateTime(2200,12,12),
                    Description = "mjkllkjhkjh",
                    OrganiserEmail = "uy@uy.com",
                    OrganiserPhoneNumber = "07123456000",
                    OrganiserFirstName = "Joan",
                    OrganiserLastName = "Blogsmith"
                }
                    }
                },
                new ParkModel {
                    ParkId = 4002,
                PostcodeZone = "LP2" ,
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
