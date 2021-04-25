using LocalParks.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LocalParks.Data.Tests
{
    public class ParkRepositoryTests
    {
        private readonly Mock<ILogger<ParkRepository>> _logger;
        private readonly IConfigurationRoot _configuration;
        private readonly DbContextOptions<ParkContext> _options;

        private readonly ParkContext _context;
        private readonly ParkRepository _repository;

        public ParkRepositoryTests()
        {
            _logger = new Mock<ILogger<ParkRepository>>();

            var myConfiguration = new Dictionary<string, string>
            {
                {"ConnectionStrings:LocalParks", "Data Source=:memory:;Version=3;New=True;"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _options = new DbContextOptionsBuilder<ParkContext>()
                .UseInMemoryDatabase(databaseName: "ParkRepositoryTests")
                .Options;

            _context = new ParkContext(_options, _configuration);

            _repository = new ParkRepository(_context, _logger.Object);
        }

        [Fact]
        public async Task WHEN_repository_is_saved_with_no_changes_THEN_false_is_returned()
        {
            // arrange

            // act

            var result = await _repository.SaveChangesAsync();

            // assert

            Assert.False(result);
        }
        [Fact]
        public async Task WHEN_new_entity_is_saved_to_repository_THEN_new_entity_is_returned()
        {
            // arrange          
            var p1 = new Park
            {
                ParkId = 1000,
                Name = "Park1",
                SizeInMetresSquared = 1000,
                Longitude = 1.00m,
                Latitude = -1.00m,
                OpeningTime = DateTime.MinValue,
                ClosingTime = DateTime.MaxValue,
                Supervisor = new Supervisor()
            };

            // act

            _repository.Add(p1);

            var result = await _repository.SaveChangesAsync();
            var parks = await _repository.GetAllParksAsync();

            // assert

            Assert.True(result);
            Assert.NotEmpty(parks);
            Assert.Contains(p1, parks);
        }
        [Fact]
        public async Task WHEN_entity_is_deleted_from_repository_THEN_entity_is_not_found()
        {
            // arrange

            _repository.Add(new Park
            {
                ParkId = 1001,
                Name = "Park1",
                SizeInMetresSquared = 1000,
                Longitude = 1.00m,
                Latitude = -1.00m,
                OpeningTime = DateTime.MinValue,
                ClosingTime = DateTime.MaxValue,
                Supervisor = new Supervisor()
            });
            _repository.SaveChangesAsync().Wait();

            var existing = await _repository.GetParkByIdAsync(1001);

            // act

            _repository.Delete(existing);

            var result = await _repository.SaveChangesAsync();
            var parks = await _repository.GetAllParksAsync();

            // assert

            Assert.True(result);
            Assert.DoesNotContain(existing, parks);
        }

        [Fact]
        public async Task WHEN_repository_is_queried_by_parkId_THEN_park_with_parkId_is_returned()
        {
            // arrange

            var p1 = new Park
            {
                ParkId = 1010,
                Name = "Park10",
                SizeInMetresSquared = 1010,
                Longitude = 1.00m,
                Latitude = -1.00m,
                OpeningTime = DateTime.MinValue,
                ClosingTime = DateTime.MaxValue,
                Supervisor = new Supervisor()
            };

            _repository.Add(p1);
            await _repository.SaveChangesAsync();

            // act

            var result1 = await _repository.GetParkByIdAsync(1010);
            var result2 = await _repository.GetParkByIdAsync(1011);

            // assert

            Assert.NotNull(result1);
            Assert.Null(result2);
            Assert.Equal(p1.Name, result1.Name);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public async Task WHEN_repository_is_queried_by_parkName_THEN_park_with_parkName_is_returned()
        {
            // arrange

            var p1 = new Park
            {
                ParkId = 1021,
                Name = "Park21 Test",
                SizeInMetresSquared = 1010,
                Longitude = 1.00m,
                Latitude = -1.00m,
                OpeningTime = DateTime.MinValue,
                ClosingTime = DateTime.MaxValue,
                Supervisor = new Supervisor()
            };

            _repository.Add(p1);
            await _repository.SaveChangesAsync();

            // act

            var result1 = await _repository.GetParkByNameAsync("Park21 Test");
            var result2 = await _repository.GetParkByNameAsync("nothing");

            // assert

            Assert.NotNull(result1);
            Assert.Null(result2);
            Assert.Equal(p1.Name, result1.Name);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public async Task WHEN_all_SportsClubs_are_requested_THEN_all_SportClubs_are_returned()
        {
            // arrange

            var s1 = new SportsClub
            {
                ClubId = 1000,
                Name = "Sport club testing one",
                Park = new Park(),
                Sport = SportType.Athletics,
                MembershipFee = 20000,
                Members = 200,
                Email = "an@email.add.ress",
                President = "someone important"
            };
            var s2 = new SportsClub
            {
                ClubId = 1001,
                Name = "Sport club testing two",
                Park = new Park(),
                Sport = SportType.Basketball,
                MembershipFee = 30000,
                Members = 300,
                Email = "another@email.add.ress",
                President = "someone even more important"
            };

            _repository.Add(s1);
            _repository.Add(s2);
            await _repository.SaveChangesAsync();

            // act

            var result = await _repository.GetAllSportsClubsAsync();

            // assert

            Assert.NotEmpty(result);
            Assert.Contains(s1, result);
            Assert.Contains(s2, result);
        }
        [Fact]
        public async Task WHEN_repostory_is_queried_for_SportsClubs_with_parkId_THEN_all_SportClubs_with_parkId_are_returned()
        {
            // arrange

            var s1 = new SportsClub
            {
                ClubId = 1010,
                Name = "Sport club testing one one",
                Park = new Park { ParkId = 3001 },
                Sport = SportType.Cricket,
                MembershipFee = 1000,
                Members = 20,
                Email = "anpark@email.add.ress",
                President = "someone important"
            };
            var s2 = new SportsClub
            {
                ClubId = 1011,
                Name = "Sport club testing two one",
                Park = new Park { ParkId = 3002 },
                Sport = SportType.Football,
                MembershipFee = 300,
                Members = 3,
                Email = "another@email.add.ress",
                President = "someone even more important"
            };

            _repository.Add(s1);
            _repository.Add(s2);
            _repository.SaveChangesAsync().Wait();

            // act

            var result1 = await _repository.GetSportsClubsByParkIdAsync(3001);
            var result2 = await _repository.GetSportsClubsByParkIdAsync(3002);
            var result3 = await _repository.GetSportsClubsByParkIdAsync(3003);

            // assert

            Assert.NotEmpty(result1);
            Assert.NotEmpty(result2);
            Assert.Empty(result3);
            Assert.Contains(s1, result1);
            Assert.Contains(s2, result2);
        }

        [Fact]
        public async Task WHEN_repostory_is_queried_for_SportsClub_with_clubId_THEN_SportClub_with_clubId_is_returned()
        {
            // arrange

            var s1 = new SportsClub
            {
                ClubId = 1020,
                Name = "Sport club testing one one",
                Park = new Park(),
                Sport = SportType.Yoga,
                MembershipFee = 1000,
                Members = 20,
                Email = "anpark@email.add.ress",
                President = "someone important"
            };
            var s2 = new SportsClub
            {
                ClubId = 1021,
                Name = "Sport club testing two one",
                Park = new Park(),
                Sport = SportType.Other,
                MembershipFee = 300,
                Members = 3,
                Email = "another@email.add.ress",
                President = "someone even more important"
            };

            _repository.Add(s1);
            _repository.Add(s2);
            _repository.SaveChangesAsync().Wait();

            // act

            var result1 = await _repository.GetSportsClubByIdAsync(1020);
            var result2 = await _repository.GetSportsClubByIdAsync(1021);
            var result3 = await _repository.GetSportsClubByIdAsync(1022);

            // assert

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Null(result3);
            Assert.Equal(s1, result1);
            Assert.Equal(s2, result2);
        }

        [Fact]
        public async Task WHEN_repostory_is_queried_for_SportsClubs_with_sportType_THEN_all_SportClubs_with_sportType_are_returned()
        {
            // arrange

            var s1 = new SportsClub
            {
                ClubId = 1030,
                Name = "Sport club testing one one",
                Park = new Park(),
                Sport = SportType.Other,
                MembershipFee = 1000,
                Members = 20,
                Email = "anpark@email.add.ress",
                President = "someone important"
            };
            var s2 = new SportsClub
            {
                ClubId = 1031,
                Name = "Sport club testing two one",
                Park = new Park(),
                Sport = SportType.Tennis,
                MembershipFee = 300,
                Members = 3,
                Email = "another@email.add.ress",
                President = "someone even more important"
            };
            var s3 = new SportsClub
            {
                ClubId = 1032,
                Name = "Sport club testing two one",
                Park = new Park(),
                Sport = SportType.Tennis,
                MembershipFee = 300,
                Members = 3,
                Email = "another@email.add.ress",
                President = "someone even more important"
            };

            _repository.Add(s1);
            _repository.Add(s2);
            _repository.Add(s3);
            _repository.SaveChangesAsync().Wait();

            // act

            var result1 = await _repository.GetSportsClubsBySportAsync(SportType.Other);
            var result2 = await _repository.GetSportsClubsBySportAsync(SportType.Tennis);
            var result3 = await _repository.GetSportsClubsBySportAsync(SportType.Bowls);

            // assert

            Assert.NotEmpty(result1);
            Assert.NotEmpty(result2);
            Assert.Null(result3);
            Assert.Contains(s1, result1);
            Assert.Contains(s2, result2);
            Assert.Contains(s3, result2);
        }

        [Fact]
        public async Task WHEN_all_Supervisors_are_requested_THEN_all_Supervisors_are_returned()
        {
            // arrange

            var s1 = new Supervisor
            {
                SupervisorId = 1000,
                Park = new Park { ParkId = 2000 },
                ParkRef = 2000,
                FirstName = "Bob",
                LastName = "Frank",
                Salary = 10000,
                StartingDate = DateTime.MinValue,
                Office = "Near park 2000",
                EmergencyNumber = "07000000000"
            };
            var s2 = new Supervisor
            {
                SupervisorId = 1001,
                Park = new Park { ParkId = 2001 },
                ParkRef = 2001,
                FirstName = "Manfred",
                LastName = "Alblot",
                Salary = 20000,
                StartingDate = DateTime.MinValue.AddDays(1),
                Office = "Near park 2001",
                EmergencyNumber = "07000000001"
            };
            var s3 = new Supervisor
            {
                SupervisorId = 999,
                Park = new Park { ParkId = 1999 },
                ParkRef = 1999,
                FirstName = "Manfred",
                LastName = "Alblot",
                Salary = 20000,
                StartingDate = DateTime.MinValue.AddDays(1),
                Office = "Near park 2001",
                EmergencyNumber = "07000000001"
            };

            _repository.Add(s1);
            _repository.Add(s2);
            await _repository.SaveChangesAsync();

            // act

            var result = await _repository.GetAllSupervisorsAsync();

            // assert

            Assert.NotEmpty(result);
            Assert.Contains(s1, result);
            Assert.Contains(s2, result);
            Assert.DoesNotContain(s3, result);
        }
        [Fact]
        public async Task WHEN_repostory_is_queried_for_Supervisor_with_parkId_THEN_Supervisor_with_parkId_is_returned()
        {
            // arrange

            var s1 = new Supervisor
            {
                SupervisorId = 1010,
                Park = new Park { ParkId = 2010 },
                ParkRef = 2010,
                FirstName = "Bob",
                LastName = "Frank",
                Salary = 10000,
                StartingDate = DateTime.MinValue,
                Office = "Near park 2000",
                EmergencyNumber = "07000000000"
            };

            _repository.Add(s1);
            await _repository.SaveChangesAsync();

            // act

            var result1 = await _repository.GetSupervisorByParkIdAsync(2010);
            var result2 = await _repository.GetSupervisorByParkIdAsync(2011);

            // assert

            Assert.NotNull(result1);
            Assert.Null(result2);
            Assert.Equal(s1.SupervisorId, result1.SupervisorId);
            Assert.NotEqual(result1, result2);
        }

        [Fact]
        public async Task WHEN_all_ParkEvents_are_requested_THEN_all_ParkEvents_are_returned()
        {
            // arrange

            var e1 = new ParkEvent
            {
                EventId = 1000,
                Name = "An event in a park",
                Park = new Park(),
                Date = DateTime.MaxValue,
                Description = "skdlfgkjsdlfkgjhsdlfkjghsdlkfjg",
                OrganiserFirstName = "Wendy",
                OrganiserLastName = "Addams",
                OrganiserEmail = "a@b.c.d",
                OrganiserPhoneNumber = "07000000000"
            };
            var e2 = new ParkEvent
            {
                EventId = 1001,
                Name = "An event in a park two",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-1),
                Description = "skdlfgkjsdlfkgjjhjhjhjhhsdlfkjghsdlkfjg",
                OrganiserFirstName = "Mandy",
                OrganiserLastName = "Yandy",
                OrganiserEmail = "lkja@blkj.clkj.lkjd",
                OrganiserPhoneNumber = "07000000000"
            };
            var e3 = new ParkEvent
            {
                EventId = 1002,
                Name = "An event in a park three",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-2),
                Description = "skdlfgkjsdlfkgjhslkjhdlfkjghsdlkfjg",
                OrganiserFirstName = "Bill",
                OrganiserLastName = "Markup",
                OrganiserEmail = "ajj@bi.ckm.dkk",
                OrganiserPhoneNumber = "07000120000"
            };

            _repository.Add(e1);
            _repository.Add(e2);
            await _repository.SaveChangesAsync();

            // act

            var result = await _repository.GetAllEventsAsync();

            // assert

            Assert.NotEmpty(result);
            Assert.Contains(e1, result);
            Assert.Contains(e2, result);
            Assert.DoesNotContain(e3, result);
        }
        [Fact]
        public async Task WHEN_repostory_is_queried_for_parkEvent_with_eventId_THEN_parkEvent_with_eventId_is_returned()
        {
            // arrange

            var e1 = new ParkEvent
            {
                EventId = 1010,
                Name = "An event in a park one one",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-10),
                Description = "skdlfgkjsdlfkgjhsdlfkjghsdlkfjg",
                OrganiserFirstName = "Wendy",
                OrganiserLastName = "Addams",
                OrganiserEmail = "a@b.c.d",
                OrganiserPhoneNumber = "07000000000"
            };
            var e2 = new ParkEvent
            {
                EventId = 1011,
                Name = "An event in a park two one",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-11),
                Description = "skdlfgkjsdlfkgjjhjhjhjhhsdlfkjghsdlkfjg",
                OrganiserFirstName = "Mandy",
                OrganiserLastName = "Yandy",
                OrganiserEmail = "lkja@blkj.clkj.lkjd",
                OrganiserPhoneNumber = "07000000000"
            };

            _repository.Add(e1);
            _repository.Add(e2);
            await _repository.SaveChangesAsync();

            // act

            var result1 = await _repository.GetEventByIdAsync(1010);
            var result2 = await _repository.GetEventByIdAsync(1011);
            var result3 = await _repository.GetEventByIdAsync(1012);

            // assert

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Null(result3);
            Assert.Equal(e1, result1);
            Assert.Equal(e2, result2);
        }

        [Fact]
        public async Task WHEN_repostory_is_queried_for_parkEvents_with_date_THEN_all_parkEvent_with_date_are_returned()
        {
            // arrange

            var e1 = new ParkEvent
            {
                EventId = 1020,
                Name = "An event in a park",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-20),
                Description = "skdlfgkjsdlfkgjhsdlfkjghsdlkfjg",
                OrganiserFirstName = "Wendy",
                OrganiserLastName = "Addams",
                OrganiserEmail = "a@b.c.d",
                OrganiserPhoneNumber = "07000000000"
            };
            var e2 = new ParkEvent
            {
                EventId = 1021,
                Name = "An event in a park two",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-21),
                Description = "skdlfgkjsdlfkgjjhjhjhjhhsdlfkjghsdlkfjg",
                OrganiserFirstName = "Mandy",
                OrganiserLastName = "Yandy",
                OrganiserEmail = "lkja@blkj.clkj.lkjd",
                OrganiserPhoneNumber = "07000000000"
            };
            var e3 = new ParkEvent
            {
                EventId = 1022,
                Name = "An event in a park three",
                Park = new Park(),
                Date = DateTime.MaxValue.AddDays(-20),
                Description = "skdlfgkjsdlfkgjhslkjhdlfkjghsdlkfjg",
                OrganiserFirstName = "Bill",
                OrganiserLastName = "Markup",
                OrganiserEmail = "ajj@bi.ckm.dkk",
                OrganiserPhoneNumber = "07000120000"
            };

            _repository.Add(e1);
            _repository.Add(e2);
            _repository.Add(e3);
            await _repository.SaveChangesAsync();

            // act

            var result1 = await _repository.GetEventsByDateAsync(DateTime.MaxValue.AddDays(-20));
            var result2 = await _repository.GetEventsByDateAsync(DateTime.MaxValue.AddDays(-21));
            var result3 = await _repository.GetEventsByDateAsync(DateTime.MaxValue.AddDays(-22));

            // assert

            Assert.NotEmpty(result1);
            Assert.NotEmpty(result2);
            Assert.Empty(result3);
            Assert.Contains(e1, result1);
            Assert.Contains(e3, result1);
            Assert.Contains(e2, result2);
            Assert.DoesNotContain(e3, result2);
        }
    }
}
