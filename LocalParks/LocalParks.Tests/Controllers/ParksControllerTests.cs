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
    public class ParksControllerTests
    {
        private readonly Mock<ILogger<ParksController>> _mockLogger;
        private readonly Mapper _mapper;
        private readonly TempDataDictionary _tempData;
        private readonly Park[] _parks;

        public ParksControllerTests()
        {
            _mockLogger = new Mock<ILogger<ParksController>>();

            var myProfile = new ParkProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _parks = GetTestParks();

            var httpContext = new DefaultHttpContext();
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        }

        [InlineData(true, 0, null)]
        [InlineData(true, 1, "one")]
        [InlineData(true, 1, "ktw")]
        [InlineData(true, 0, "xxx")]
        [Theory]
        public async Task ShouldReturnIndexViewWithParksModel(bool isAny, int missingParks, string searchterm)
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetAllParksAsync()).Returns(Task.FromResult(_parks));

            var controller = new ParksController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Index(searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(isAny, model.Any());
            Assert.Equal(_parks.Length - missingParks, model.Count());
        }

        [InlineData(null, null, null)]
        [InlineData("one", null, "One")]
        [InlineData("ktw", null, "kTw")]
        [InlineData(null, "No Matches found", "xxx")]
        [Theory]
        public async Task ShouldSetTempDataWithSearchterm(string tdFilter, string tdMatches, string searchterm)
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetAllParksAsync()).Returns(Task.FromResult(_parks));

            var controller = new ParksController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Index(searchterm);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ParkModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(tdFilter, viewResult.TempData["Filter"]);
            Assert.Equal(tdMatches, viewResult.TempData["Matches"]);
        }

        [InlineData(0, 5001)]
        [InlineData(1, 5002)]
        [Theory]
        public async Task ShouldReturnDetailsView(int parkPos, int parkId)
        {
            var mockRepo = new Mock<IParkRepository>();
            mockRepo.Setup(r => r.GetParkByIdAsync(parkId))
                .Returns(Task.FromResult(_parks.Where(p => p.ParkId == parkId).First()));

            var controller = new ParksController(_mockLogger.Object, mockRepo.Object, _mapper)
            {
                TempData = _tempData
            };

            var result = await controller.Details(parkId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ParkModel>(
                viewResult.ViewData.Model);

            Assert.Null(viewResult.ViewName);
            Assert.Equal(model.ParkId, _parks[parkPos].ParkId);
        }

        internal static Park[] GetTestParks()
        {
            return new Park[]
            {
                new Park {
                    ParkId = 5001,
                Postcode = new Postcode {Zone = "LP1" },
                    Name = "TestingParkOne",
                    SizeInMetresSquared = 20000,
                    Longitude = 12.345m,
                    Latitude = 5.4321m,
                    OpeningTime = new DateTime (2000,1,1,7,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,19,0,0 ),
                    Supervisor = new Supervisor{ SupervisorId = 5101, ParkRef = 5001}
                },
                new Park {
                    ParkId = 5002,
                    Postcode = new Postcode {Zone = "LP2" },
                    Name = "TestingParkTwo",
                    SizeInMetresSquared = 1000,
                    Longitude = 1.2345m,
                    Latitude = 54.321m,
                    OpeningTime = new DateTime (2000,1,1,6,0,0 ),
                    ClosingTime =new DateTime (2000,1,1,20,0,0 ),
                    Supervisor = new Supervisor{ SupervisorId = 5102, ParkRef = 5002}
                }
            };
        }
    }
}
