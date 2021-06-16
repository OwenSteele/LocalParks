//using AutoMapper;
//using LocalParks.Controllers;
//using LocalParks.Core;
//using LocalParks.Data;
//using LocalParks.Models;
//using LocalParks.Services;
//using LocalParks.Services.View;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace LocalParks.Tests.Controllers
//{
//    public class ParkEventsServiceTests
//    {
//        private readonly Mock<ILogger<ParkEventsController>> _mockLogger;

//        private readonly Mock<ISelectListService> _mockListService;
//        private readonly Mock<IAuthenticationService> _mockAuthService;
//        private readonly Mock<IParkEventsService> _mockService;
//        private readonly Mock<ISortingService> _mockSortService;

//        private readonly TempDataDictionary _tempData;
//        private readonly ParkEvent[] _events;

//        public ParkEventsServiceTests()
//        {
//            _mockLogger = new Mock<ILogger<ParkEventsController>>();
//            _mockListService = new Mock<ISelectListService>();
//            _mockAuthService = new Mock<IAuthenticationService>();
//            _mockService = new Mock<IParkEventsService>();
//            _mockSortService = new Mock<ISortingService>();

//            var myProfile = new ParkProfile();
//            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

//            _events = GetTestParkEvents();

//            var httpContext = new DefaultHttpContext();
//            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
//        }

//        private ParkEventsController CreateController() => new ParkEventsController(
//            _mockLogger.Object,
//                _mockService.Object, 
//                _mockAuthService.Object, 
//                _mockListService.Object)
//            {
//                TempData = _tempData
//            };

//        private Mock<IParkRepository> CreateRepo(ParkEvent[] events = null) 
//        {
//            events ??= _events;

//            var repo = new Mock<IParkRepository>();
//            repo.Setup(r => r.GetAllEventsAsync(true)).Returns(Task.FromResult(events));

//            return repo;
//        }
//        private Mock<IParkRepository> CreateRepo(Park[] parks)
//        {
//            var repo = new Mock<IParkRepository>();
//            repo.Setup(r => r.GetAllParksAsync(true)).Returns(Task.FromResult(parks));

//            return repo;
//        }

//        [Fact]
//        public async Task WHEN_Index_view_THEN_Index_view_is_returned_with_ParkEventModel()
//        {
//            _ = CreateRepo();

//            var controller = CreateController();

//            var result = await controller.Index();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
//                viewResult.ViewData.Model);

//            Assert.True(model.Any());
//        }

//        [InlineData(true, 0, null)]
//        [InlineData(true, 1, "a BIg")]
//        [InlineData(true, 1, "another")]
//        [InlineData(true, 0, "xxx")]
//        [Theory]
//        public async Task WHEN_Filter_view_is_requested_with_searchTerm_THEN_Index_view_is_returned_only_with_matching_events(
//            bool isAny, int missingEvents, string searchterm)
//        {
//            _ = CreateRepo();

//            var controller = CreateController();

//            var result = await controller.Filter(_mockSortService.Object, searchterm);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
//                viewResult.ViewData.Model);

//            Assert.Equal(isAny, model.Any());
//            Assert.Equal(_events.Length - missingEvents, model.Count());
//        }

//        [InlineData(null, null, null)]
//        [InlineData("a big", null, "a BIg")]
//        [InlineData("another", null, "anOTHer")]
//        [InlineData(null, "No Matches found", "xxx")]
//        [Theory]
//        public async Task WHEN_Filter_view_is_requested_with_searchTerm_THEN_Index_view_TempData_containing_searchTerm(
//            string tdFilter, string tdMatches, string searchterm)
//        {
//            var mockRepo = CreateRepo();

//            var controller = CreateController();

//            var result = await controller.Filter(_mockSortService.Object, searchterm);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
//                viewResult.ViewData.Model);

//            Assert.Equal(tdFilter, viewResult.TempData["Filter"]);
//            Assert.Equal(tdMatches, viewResult.TempData["Matches"]);
//        }

//        [InlineData(0, 7001)]
//        [InlineData(1, 7002)]
//        [Theory]
//        public async Task WHEN_details_view_is_requested_with_eventId_THEN_details_view_is_returned_containing_matching_ParkEventModel(
//            int parkPos, int eventId)
//        {
//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetEventByIdAsync(eventId))
//                .Returns(Task.FromResult(_events.Where(p => p.EventId == eventId).First()));

//            var controller = CreateController();

//            var result = await controller.Details(eventId);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.Null(viewResult.ViewName);
//            Assert.Equal(_events[parkPos].Park.ParkId, model.ParkId);
//        }

//        [Fact]
//        public async Task WHEN_Edit_view_is_requested_without_parameters_THEN_edit_view_without_a_model_is_returned()
//        {
//            var mockRepo = CreateRepo(GetTestParks());

//            var controller = CreateController();

//            var result = await controller.Edit();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.NotNull(viewResult.ViewData["Parks"]);
//            Assert.Null(model.Name);
//        }

//        [InlineData(7001)]
//        [InlineData(7002)]
//        [Theory]
//        public async Task WHEN_Edit_view_is_requested_with_eventId_THEN_edit_view_containing_matching_ParkEventModel_is_returned(
//            int eventId)
//        {

//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetEventByIdAsync(eventId))
//                .Returns(Task.FromResult(_events.Where(p => p.EventId == eventId).First()));

//            var controller = CreateController();

//            var result = await controller.Edit(eventId);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.Null(viewResult.ViewData["Parks"]);
//            Assert.NotNull(model.Name);
//            Assert.Equal(eventId, model.EventId);
//        }

//        [Fact]
//        public async Task WHEN_Edit_View_is_requested_with_nonexistant_parkID_THEN_edit_view_without_a_model_is_returned()
//        {
//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetParkByIdAsync(5001)).Returns(Task.FromResult(GetTestParks()[0]));

//            var controller = CreateController();

//            controller.Event = new ParkEventModel { ParkId = 11111 };

//            var result = await controller.Edit();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.NotNull(viewResult.ViewData["Parks"]);
//            Assert.Null(model.Name);
//        }

//        [Fact]
//        public async Task WHEN_Edit_view_is_requested_with_date_that_is_taken_THEN_edit_view_is_returned_with_ViewData_Error()
//        {
//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetParkByIdAsync(5001)).Returns(Task.FromResult(GetTestParks()[0]));

//            var controller = CreateController();

//            controller.Event = new ParkEventModel
//            {
//                ParkId = 5001,
//                ParkName = "TestingParkOne",
//                EventId = 9001,
//                Name = "NewParkEventTest",
//                Date = new DateTime(2200, 12, 12)
//            };

//            var result = await controller.Edit();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.NotNull(viewResult.ViewData["Parks"]);
//            Assert.Equal("An Event is already booked for this date at TestingParkOne", viewResult.ViewData["DateTaken"]);
//            Assert.NotNull(model.Name);
//        }
//        [Fact]
//        public async Task WHEN_Edit_view_is_requested_with_model_error_THEN_edit_view_is_returned_with_an_invalid_model()
//        {
//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetParkByIdAsync(5001)).Returns(Task.FromResult(GetTestParks()[0]));

//            var controller = CreateController();

//            controller.Event = new();

//            controller.ModelState.AddModelError("Name", "Required");

//            var result = await controller.Edit();

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.NotNull(viewResult.ViewData["Parks"]);
//            Assert.Null(model.Name);
//        }

//        [InlineData(7001)]
//        [InlineData(7002)]
//        [Theory]
//        public async Task ShouldRedirecttoDetailsFromDeleteAction(int eventId)
//        {

//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetEventByIdAsync(eventId))
//                .Returns(Task.FromResult(_events.Where(p => p.EventId == eventId).First()));

//            var controller = CreateController();

//            var result = await controller.Delete(eventId);

//            var actionResult = Assert.IsType<RedirectToActionResult>(result);
//            Assert.Equal("ParkEvents", actionResult.ControllerName);
//            Assert.Equal("Details", actionResult.ActionName);
//        }

//        [Fact]
//        public async Task ShouldReturnDeleteViewNotConfirmed()
//        {
//            var date = new DateTime(2200, 1, 1);
//            var parkId = 8001;

//            var mockRepo = new Mock<IParkRepository>();
//            mockRepo.Setup(r => r.GetEventByParkIdByDateAsync(parkId, date))
//                .Returns(Task.FromResult(_events.Where(p => p.Park.ParkId == parkId).First()));

//            var controller = CreateController();

//            var result = await controller.Delete(parkId, date);

//            var viewResult = Assert.IsType<ViewResult>(result);
//            var model = Assert.IsAssignableFrom<ParkEventModel>(
//                viewResult.ViewData.Model);

//            Assert.NotNull(model.Name);
//            Assert.Equal(parkId, model.ParkId);
//            Assert.Equal(date, model.Date);
//        }

//        private static ParkEvent[] GetTestParkEvents()
//        {
//            return new ParkEvent[]
//            {
//                new ParkEvent
//                {
//                    EventId = 7001,
//                    Name = "A big event",
//                    Park = new Park { ParkId = 8001, Name = "TestingParkOne for Event"},
//                    Date = new DateTime(2200,1,1),
//                    Description = "djfglshdflgkjsdfhglkjsdhfg",
//                    OrganiserEmail = "fdg@sdf.coum",
//                    OrganiserPhoneNumber = "07123456789",
//                    OrganiserFirstName = "Joe",
//                    OrganiserLastName = "Bloggs"
//                },
//                new ParkEvent
//                {
//                    EventId = 7002,
//                    Name = "Another big event",
//                    Park = new Park { ParkId = 8002, Name = "TestingParkTwo for Event"},
//                    Date = new DateTime(2200,1,2),
//                    Description = "mjkllkjhkjh",
//                    OrganiserEmail = "uy@uy.com",
//                    OrganiserPhoneNumber = "07123456000",
//                    OrganiserFirstName = "Joan",
//                    OrganiserLastName = "Blogsmith"
//                }
//            };
//        }
//        private static Park[] GetTestParks()
//        {
//            return new Park[]
//            {
//                new Park {
//                    ParkId = 4001,
//                Postcode = new Postcode {Zone = "LP1" },
//                    Name = "TestingParkOne",
//                    SizeInMetresSquared = 20000,
//                    Longitude = 12.345m,
//                    Latitude = 5.4321m,
//                    OpeningTime = new DateTime (2000,1,1,7,0,0 ),
//                    ClosingTime =new DateTime (2000,1,1,19,0,0 ),
//                    Supervisor = new Supervisor{ SupervisorId = 5101, ParkRef = 5001},
//                    Events = new ParkEvent[]
//                    {
//                        new ParkEvent
//                {
//                    EventId = 14001,
//                    Name = "Another big event",
//                    Date = new DateTime(2200,12,12),
//                    Description = "mjkllkjhkjh",
//                    OrganiserEmail = "uy@uy.com",
//                    OrganiserPhoneNumber = "07123456000",
//                    OrganiserFirstName = "Joan",
//                    OrganiserLastName = "Blogsmith"
//                }
//                    }
//                },
//                new Park {
//                    ParkId = 4002,
//                Postcode = new Postcode {Zone = "LP2" },
//                    Name = "TestingParkTwo",
//                    SizeInMetresSquared = 1000,
//                    Longitude = 1.2345m,
//                    Latitude = 54.321m,
//                    OpeningTime = new DateTime (2000,1,1,6,0,0 ),
//                    ClosingTime =new DateTime (2000,1,1,20,0,0 ),
//                    Supervisor = new Supervisor{ SupervisorId = 5102, ParkRef = 5002}
//                }
//            };
//        }
//    }
//}
