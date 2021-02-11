using AutoMapper;
using LocalParks.Controllers;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LocalParks.Tests.Controllers
{
    public class ParkEventsControllerTests
    {
        private readonly Mock<ILogger<ParkEventsController>> _mockLogger;
        private readonly Mapper _mapper;
        private readonly TempDataDictionary _tempData;
        private readonly ParkEvent[] _events;

        public ParkEventsControllerTests()
        {
            _mockLogger = new Mock<ILogger<ParkEventsController>>();

            var myProfile = new ParkProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _events = GetTestParkEvents();

            var httpContext = new DefaultHttpContext();
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        }

        [InlineData(true, 0, null)]
        [InlineData(true, 1, "a BIg")]
        [InlineData(true, 1, "another")]
        [InlineData(true, 0, "xxx")]
        [Theory]
        public async Task ShouldReturnIndexViewWithParkEventsModel(bool isAny, int missingEvents, string searchterm)
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetAllEventsAsync()).Returns(Task.FromResult(_events));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Index(searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(isAny, model.Any());
            Assert.Equal(_events.Length - missingEvents, model.Count());
        }

        [InlineData(null, null, null)]
        [InlineData("a big", null, "a BIg")]
        [InlineData("another", null, "anOTHer")]
        [InlineData(null, "No Matches found", "xxx")]
        [Theory]
        public async Task ShouldReturnIndexSetTempDataWithSearchterm(string tdFilter, string tdMatches, string searchterm)
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetAllEventsAsync()).Returns(Task.FromResult(_events));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Index(searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkEventModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(tdFilter, viewResult.TempData["Filter"]);
            Assert.Equal(tdMatches, viewResult.TempData["Matches"]);
        }

        [InlineData(0, 8001, 2200, 1, 1)]
        [InlineData(1, 8002, 2200, 1, 2)]
        [Theory]
        public async Task ShouldReturnDetailsView(int parkPos, int parkId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetEventByParkIdByDateAsync(parkId, date))
                .Returns(Task.FromResult(_events.Where(p => p.Park.ParkId == parkId).First()));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Details(parkId, date);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.Null(viewResult.ViewName);
            Assert.Equal(_events[parkPos].Park.ParkId, model.ParkId);
        }

        [Fact]
        public async Task ShouldReturnEmptyModelFromEditAction()
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetAllParksAsync()).Returns(Task.FromResult(GetTestParks()));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Edit(0, DateTime.MinValue);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Null(model.Name);
        }

        [InlineData(8001, 2200, 1, 1)]
        [InlineData(8002, 2200, 1, 2)]
        [Theory]
        public async Task ShouldReturnCompleteModelFromEditAction(int parkId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetEventByParkIdByDateAsync(parkId, date))
                .Returns(Task.FromResult(_events.Where(p => p.Park.ParkId == parkId).First()));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Edit(parkId, date);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.Null(viewResult.ViewData["Parks"]);
            Assert.NotNull(model.Name);
            Assert.Equal(parkId, model.ParkId);
            Assert.Equal(date, model.Date);
        }

        [Fact]
        public async Task ShouldReturnEditViewwithNoMatch()
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetParkByIdAsync(5001)).Returns(Task.FromResult(GetTestParks()[0]));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData,
                Event = new ParkEventModel
                {
                    ParkId = 11111
                }
            };

            var result = await controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Null(model.Name);
        }

        [Fact]
        public async Task ShouldReturnEditViewwithDateTaken()
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetParkByIdAsync(5001)).Returns(Task.FromResult(GetTestParks()[0]));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData,
                Event = new ParkEventModel
                {
                    ParkId = 5001,
                    ParkName = "TestingParkOne",
                    EventId = 9001,
                    Name = "NewParkEventTest",
                    Date = new DateTime(2200, 12, 12)
                }
            };

            var result = await controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Equal("An Event is already booked for this date at TestingParkOne", viewResult.ViewData["DateTaken"]);
            Assert.NotNull(model.Name);
        }
        [Fact]
        public async Task ShouldReturnEditViewwithInvalidModel()
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetParkByIdAsync(5001)).Returns(Task.FromResult(GetTestParks()[0]));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData,
                Event = new ParkEventModel()
            };

            controller.ModelState.AddModelError("Name", "Required");

            var result = await controller.Edit();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(viewResult.ViewData["Parks"]);
            Assert.Null(model.Name);
        }

        [InlineData(8001, 2200, 1, 1)]
        [InlineData(8002, 2200, 1, 2)]
        [Theory]
        public async Task ShouldRedirecttoDetailsFromDeleteAction(int parkId, int year, int month, int day)
        {
            var date = new DateTime(year, month, day);

            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetEventByParkIdByDateAsync(parkId, date))
                .Returns(Task.FromResult(_events.Where(p => p.Park.ParkId == parkId).First()));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Delete(parkId, date, true);

            var actionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ParkEvents", actionResult.ControllerName);
            Assert.Equal("Details", actionResult.ActionName);
        }

        [Fact]
        public async Task ShouldReturnDeleteViewNotConfirmed()
        {
            var date = new DateTime(2200, 1, 1);
            var parkId = 8001;

            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetEventByParkIdByDateAsync(parkId, date))
                .Returns(Task.FromResult(_events.Where(p => p.Park.ParkId == parkId).First()));

            var controller = new ParkEventsController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Delete(parkId, date);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkEventModel>(
                viewResult.ViewData.Model);

            Assert.NotNull(model.Name);
            Assert.Equal(parkId, model.ParkId);
            Assert.Equal(date, model.Date);
        }

        private static ParkEvent[] GetTestParkEvents()
        {
            return new ParkEvent[]
            {
                new ParkEvent
                {
                    EventId = 7001,
                    Name = "A big event",
                    Park = new Park { ParkId = 8001, Name = "TestingParkOne for Event"},
                    Date = new DateTime(2200,1,1),
                    Description = "djfglshdflgkjsdfhglkjsdhfg",
                    OrganiserEmail = "fdg@sdf.coum",
                    OrganiserPhoneNumber = "07123456789",
                    OrganiserFirstName = "Joe",
                    OrganiserLastName = "Bloggs"
                },
                new ParkEvent
                {
                    EventId = 7002,
                    Name = "Another big event",
                    Park = new Park { ParkId = 8002, Name = "TestingParkTwo for Event"},
                    Date = new DateTime(2200,1,2),
                    Description = "mjkllkjhkjh",
                    OrganiserEmail = "uy@uy.com",
                    OrganiserPhoneNumber = "07123456000",
                    OrganiserFirstName = "Joan",
                    OrganiserLastName = "Blogsmith"
                }
            };
        }
        private static Park[] GetTestParks()
        {
            return new Park[]
            {
                new Park {
                    ParkId = 4001,
                Postcode = new Postcode {PostcodeZone = "LP1" },
                    Name = "TestingParkOne",
                    SizeInMetresSquared = 20000,
                    Longitude = 12.345,
                    Latitude = 5.4321,
                    OpeningTime = new DateTime (2000,1,1,7,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,19,0,0 ),
                    Supervisor = new Supervisor{ SupervisorId = 5101, ParkRef = 5001},
                    Events = new ParkEvent[]
                    {
                        new ParkEvent
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
                new Park {
                    ParkId = 4002,
                Postcode = new Postcode {PostcodeZone = "LP2" },
                    Name = "TestingParkTwo",
                    SizeInMetresSquared = 1000,
                    Longitude = 1.2345,
                    Latitude = 54.321,
                    OpeningTime = new DateTime (2000,1,1,6,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,20,0,0 ),
                    Supervisor = new Supervisor{ SupervisorId = 5102, ParkRef = 5002}
                }
            };
        }
    }
}
