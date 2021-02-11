using LocalParks.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace LocalParks.Data
{
    public class ParkContext : DbContext
    {
        private readonly IConfiguration _config;

        public ParkContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        public DbSet<Park> Parks { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<SportsClub> SportsClubs { get; set; }
        public DbSet<ParkEvent> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                builder.UseSqlServer(_config.GetConnectionString("LocalParks"));
        }

        protected override void OnModelCreating(ModelBuilder bd)
        {

            //defined relationships
            bd.Entity<Postcode>().HasMany(z => z.Parks).WithOne(p => p.Postcode);
            bd.Entity<Park>().HasOne(p => p.Supervisor).WithOne(s => s.Park)
                .HasForeignKey<Supervisor>(s => s.ParkRef);
            bd.Entity<Park>().HasMany(p => p.SportClubs).WithOne(c => c.Park);
            bd.Entity<Park>().HasMany(p => p.Events).WithOne(e => e.Park);

            //defined keys
            bd.Entity<Postcode>().HasKey(k => k.PostcodeZone);
            bd.Entity<Park>().HasKey(k => k.ParkId);
            bd.Entity<Supervisor>().HasKey(k => k.SupervisorId);
            bd.Entity<SportsClub>().HasKey(k => k.ClubId);
            bd.Entity<ParkEvent>().HasKey(k => k.EventId);

            //defined starting data
            bd.Entity<Postcode>()
                .HasData(new
                {
                    PostCodeZone = "LP1",
                    Neighbourhood = "Ringhurst",
                    Population = 274987
                }, new
                {
                    PostCodeZone = "LP2",
                    Neighbourhood = "MapleDown",
                    Population = 16570
                }, new
                {
                    PostCodeZone = "LP3",
                    Neighbourhood = "Alterdon",
                    Population = 98105
                }, new
                {
                    PostCodeZone = "LP4",
                    Neighbourhood = "Rakeley",
                    Population = 7654
                }, new
                {
                    PostCodeZone = "PF10",
                    Neighbourhood = "Chapterborough",
                    Population = 100311
                }, new
                {
                    PostCodeZone = "PF11",
                    Neighbourhood = "MarketTown",
                    Population = 3948
                }, new
                {
                    PostCodeZone = "PF12",
                    Neighbourhood = "Snowbush",
                    Population = 10334
                }, new
                {
                    PostCodeZone = "PF13",
                    Neighbourhood = "Romsby",
                    Population = 12808
                }, new
                {
                    PostCodeZone = "PF14",
                    Neighbourhood = "Accreton",
                    Population = 6510
                }, new
                {
                    PostCodeZone = "PF15",
                    Neighbourhood = "Caelfall",
                    Population = 968
                }
                );

            bd.Entity<Park>()
                .HasData(new
                {
                    ParkId = 1,
                    PostcodeZone = "LP4",
                    Name = "Leafy Meadows",
                    SizeInMetresSquared = 200000,
                    Longitude = 40.34857,
                    Latitude = 0.384756,
                    OpeningTime = new DateTime(2021, 1, 1, 7, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 19, 0, 0),
                    SupervisorId = 1
                }, new
                {
                    ParkId = 2,
                    Postcode = "PF15",
                    Name = "Middlesbrook Park",
                    SizeInMetresSquared = 85000,
                    Longitude = 38.8765,
                    Latitude = 0.18798,
                    OpeningTime = new DateTime(2021, 1, 1, 6, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 22, 0, 0),
                    SupervisorId = 2
                }, new
                {
                    ParkId = 3,
                    Postcode = "PF10",
                    Name = "Mirror Lake Park",
                    SizeInMetresSquared = 2000,
                    Longitude = 35.1034,
                    Latitude = 2.4678,
                    OpeningTime = new DateTime(2021, 1, 1, 9, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 23, 0, 0),
                    SupervisorId = 3
                }, new
                {
                    ParkId = 4,
                    Postcode = "LP2",
                    Name = "Shadow Grounds",
                    SizeInMetresSquared = 26500,
                    Longitude = 41.0301,
                    Latitude = -2.9088,
                    OpeningTime = new DateTime(2021, 1, 1, 8, 30, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 16, 30, 0),
                    SupervisorId = 4
                }, new
                {
                    ParkId = 4,
                    Postcode = "LP4",
                    Name = "Shadow Grounds",
                    SizeInMetresSquared = 26500,
                    Longitude = 41.6545,
                    Latitude = -1.5640,
                    OpeningTime = new DateTime(2021, 1, 1, 8, 30, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 16, 30, 0),
                    SupervisorId = 4
                }, new
                {
                    ParkId = 5,
                    Postcode = "PF13",
                    Name = "Sapphire Gardens",
                    SizeInMetresSquared = 6000,
                    Longitude = 38.999,
                    Latitude = -5.938,
                    OpeningTime = new DateTime(2021, 1, 1, 8, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 18, 30, 0),
                    SupervisorId = 5
                }, new
                {
                    ParkId = 6,
                    Postcode = "PF13",
                    Name = "Fletcher Plaza",
                    SizeInMetresSquared = 70000,
                    Longitude = 39.7145,
                    Latitude = -4.123,
                    OpeningTime = new DateTime(2021, 1, 1, 5, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 20, 30, 0),
                    SupervisorId = 6
                }
                );

            bd.Entity<Supervisor>()
                .HasData(new
                {
                    SupervisorId = 1,
                    ParkId = 1,
                    ParkRef = 1,
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Salary = (double)35000,
                    StartingDate = new DateTime(1980, 7, 13),
                    Office = "123 Chestnut Drive",
                    EmergencyNumber = "07654321000"

                }, new
                {
                    SupervisorId = 2,
                    ParkId = 2,
                    ParkRef = 2,
                    FirstName = "Anne",
                    LastName = "Smith",
                    Salary = (double)32000,
                    StartingDate = new DateTime(1960, 1, 21),
                    Office = "Parkside Cottage, Park Avenue",
                    EmergencyNumber = "07685940321"
                }, new
                {
                    SupervisorId = 3,
                    ParkId = 3,
                    ParkRef = 3,
                    FirstName = "Robert",
                    LastName = "Bob",
                    Salary = (double)35500,
                    StartingDate = new DateTime(1986, 8, 11),
                    Office = "Bramble side, Supervisor Street",
                    EmergencyNumber = "0784368711"
                }, new
                {
                    SupervisorId = 4,
                    ParkId = 4,
                    ParkRef = 4,
                    FirstName = "Yohan",
                    LastName = "Blake",
                    Salary = (double)29000,
                    StartingDate = new DateTime(2019, 3, 5),
                    Office = "1 Park Lane",
                    EmergencyNumber = "0762985702"
                }, new
                {
                    SupervisorId = 5,
                    ParkId = 5,
                    ParkRef = 5,
                    FirstName = "Michael",
                    LastName = "Mitchell",
                    Salary = (double)34000,
                    StartingDate = new DateTime(2016, 3, 5),
                    Office = "154 Wednesday drive",
                    EmergencyNumber = "0723096876"
                }, new
                {
                    SupervisorId = 6,
                    ParkId = 6,
                    ParkRef = 6,
                    FirstName = "Daniel",
                    LastName = "Cotting",
                    Salary = (double)26350,
                    StartingDate = new DateTime(2011, 9, 14),
                    Office = "Little Cottage, Ramping Lane",
                    EmergencyNumber = "0787876876"
                }
                );

            bd.Entity<SportsClub>()
                .HasData(new
                {
                    ClubId = 1,
                    Name = "Curling Polo Club",
                    ParkId = 1,
                    MembershipFee = (double)20000,
                    Sport = SportType.Other,
                    Members = 103,
                    Website = "CPCPolo-OS.co.uk",
                    Email = "CPCPolo@owensteele.co.uk",
                    President = "Matthrew Albright"
                },
                new
                {
                    ClubId = 2,
                    Name = "Meadow RFC",
                    ParkId = 1,
                    MembershipFee = (double)150,
                    Sport = SportType.Rugby,
                    Members = 20,
                    Website = "Meadow-Rugby-OS.co.uk",
                    Email = "Contact.Meadow-Rugby@owensteele.co.uk",
                    President = "Jess Hampston"
                },
                new
                {
                    ClubId = 3,
                    Name = "Chesterly Cricket Club",
                    ParkId = 2,
                    MembershipFee = (double)330,
                    Sport = SportType.Cricket,
                    Members = 67,
                    Website = "ChesterlyCricketClub-OS.co.uk",
                    Email = "Admin.ChesterlyCricket@owensteele.co.uk",
                    President = "Linda Paul"
                });

            bd.Entity<ParkEvent>()
                .HasData(new
                {
                    EventId = 1,
                    Name = "Arts and Crafts Fair",
                    ParkId = 1,
                    Date = new DateTime(2021, 8, 18),
                    Description = "A fair for everyone, for hobbyists and professionals to come together and meet.",
                    OrganiserFirstName = "Joan",
                    OrganiserLastName = "Roberts",
                    OrganiserEmail = "JoanRoberts@owensteele.co.uk",
                    OrganiserPhoneNumber = "07123456789"
                }, new
                {
                    EventId = 2,
                    Name = "Egg and Spoon Race",
                    ParkId = 2,
                    Date = new DateTime(2021, 6, 3),
                    Description = "Come and race other from the village, egg and spoons provided!",
                    OrganiserFirstName = "Dan",
                    OrganiserLastName = "Jackson",
                    OrganiserEmail = "DanJackson@owensteele.co.uk",
                    OrganiserPhoneNumber = "0700345876"
                }, new
                {
                    EventId = 3,
                    Name = "Pet adoption Clinic",
                    ParkId = 2,
                    Date = new DateTime(2021, 7, 19),
                    Description = "Come and adopt a new feline or canine friend, all looking for good homes.",
                    OrganiserFirstName = "Mark",
                    OrganiserLastName = "Davis",
                    OrganiserEmail = "MarkDavis@owensteele.co.uk",
                    OrganiserPhoneNumber = "01196596691"
                });
        }
    }
}