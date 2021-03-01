using LocalParks.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace LocalParks.Data
{
    public class ParkContext : IdentityDbContext<LocalParksUser>
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
        public DbSet<Postcode> Postcodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
                builder.UseSqlServer(_config.GetConnectionString("LocalParks"));
        }

        protected override void OnModelCreating(ModelBuilder bd)
        {
            // LocalParksUser
            base.OnModelCreating(bd);

            // defined relationships
            bd.Entity<Postcode>().HasMany(p => p.Parks).WithOne(z => z.Postcode)
                .HasForeignKey(z => z.PostcodeZone);
            bd.Entity<Park>().HasOne(p => p.Supervisor).WithOne(s => s.Park)
                .HasForeignKey<Supervisor>(s => s.ParkRef);
            bd.Entity<Park>().HasMany(p => p.SportClubs).WithOne(c => c.Park);
            bd.Entity<Park>().HasMany(p => p.Events).WithOne(e => e.Park)
                .HasForeignKey(e => e.ParkId);
            bd.Entity<LocalParksUser>().HasMany(u => u.OrganisedEvents).WithOne(e => e.User);

            // defined keys
            bd.Entity<Postcode>().HasKey(k => k.Zone);
            bd.Entity<Park>().HasKey(k => k.ParkId);
            bd.Entity<Supervisor>().HasKey(k => k.SupervisorId);
            bd.Entity<SportsClub>().HasKey(k => k.ClubId);
            bd.Entity<ParkEvent>().HasKey(k => k.EventId);

            // decimal properties
            bd.Entity<SportsClub>().Property(c => c.MembershipFee)
                .HasColumnType("decimal(18,4)");
            bd.Entity<Supervisor>().Property(s => s.Salary)
                .HasColumnType("decimal(18,4)");
            bd.Entity<Park>().Property(p => p.Latitude)
                .HasColumnType("decimal(18,4)");
            bd.Entity<Park>().Property(p => p.Longitude)
                .HasColumnType("decimal(18,4)");

            // entity data
            bd.Entity<Postcode>()
                .HasData(new
                {
                    Zone = "LP1",
                    Neighbourhood = "Ringhurst",
                    Population = 274987L
                }, new
                {
                    Zone = "LP2",
                    Neighbourhood = "MapleDown",
                    Population = 16570L
                }, new
                {
                    Zone = "LP3",
                    Neighbourhood = "Alterdon",
                    Population = 98105L
                }, new
                {
                    Zone = "LP4",
                    Neighbourhood = "Rakeley",
                    Population = 7654L
                }, new
                {
                    Zone = "PF10",
                    Neighbourhood = "Chapterborough",
                    Population = 100311L
                }, new
                {
                    Zone = "PF11",
                    Neighbourhood = "MarketTown",
                    Population = 3948L
                }, new
                {
                    Zone = "PF12",
                    Neighbourhood = "Snowbush",
                    Population = 10334L
                }, new
                {
                    Zone = "PF13",
                    Neighbourhood = "Romsby",
                    Population = 12808L
                }, new
                {
                    Zone = "PF14",
                    Neighbourhood = "Accreton",
                    Population = 6510L
                }, new
                {
                    Zone = "PF15",
                    Neighbourhood = "Caelfall",
                    Population = 968L
                }
                );

            bd.Entity<Park>()
                .HasData(new
                {
                    ParkId = 1,
                    PostcodeZone = "LP4",
                    Name = "Leafy Meadows",
                    SizeInMetresSquared = 200000,
                    Latitude = (decimal)40.34857,
                    Longitude = (decimal)0.384756,
                    OpeningTime = new DateTime(2021, 1, 1, 7, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 19, 0, 0),
                    SupervisorId = 1
                }, new
                {
                    ParkId = 2,
                    PostcodeZone = "PF15",
                    Name = "Middlesbrook Park",
                    SizeInMetresSquared = 85000,
                    Latitude = (decimal)38.8765,
                    Longitude = (decimal)0.18798,
                    OpeningTime = new DateTime(2021, 1, 1, 6, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 22, 0, 0),
                    SupervisorId = 2
                }, new
                {
                    ParkId = 3,
                    PostcodeZone = "PF10",
                    Name = "Mirror Lake Park",
                    SizeInMetresSquared = 2000,
                    Latitude = (decimal)35.1034,
                    Longitude = (decimal)2.4678,
                    OpeningTime = new DateTime(2021, 1, 1, 9, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 23, 0, 0),
                    SupervisorId = 3
                }, new
                {
                    ParkId = 7,
                    PostcodeZone = "LP2",
                    Name = "Shadow Grounds",
                    SizeInMetresSquared = 26500,
                    Latitude = (decimal)41.0301,
                    Longitude = (decimal)-2.9088,
                    OpeningTime = new DateTime(2021, 1, 1, 8, 30, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 16, 30, 0),
                    SupervisorId = 7
                }, new
                {
                    ParkId = 4,
                    PostcodeZone = "LP4",
                    Name = "Shadow Grounds",
                    SizeInMetresSquared = 26500,
                    Latitude = (decimal)41.6545,
                    Longitude = (decimal)-1.5640,
                    OpeningTime = new DateTime(2021, 1, 1, 8, 30, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 16, 30, 0),
                    SupervisorId = 4
                }, new
                {
                    ParkId = 5,
                    PostcodeZone = "PF13",
                    Name = "Sapphire Gardens",
                    SizeInMetresSquared = 6000,
                    Latitude = (decimal)38.999,
                    Longitude = (decimal)-5.938,
                    OpeningTime = new DateTime(2021, 1, 1, 8, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 18, 30, 0),
                    SupervisorId = 5
                }, new
                {
                    ParkId = 6,
                    PostcodeZone = "PF13",
                    Name = "Fletcher Plaza",
                    SizeInMetresSquared = 70000,
                    Latitude = (decimal)39.7145,
                    Longitude = (decimal)-4.123,
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
                    Salary = (decimal)35000m,
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
                    Salary = (decimal)32000m,
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
                    Salary = (decimal)35500m,
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
                    Salary = (decimal)29000m,
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
                    Salary = (decimal)34000m,
                    StartingDate = new DateTime(2016, 11, 5),
                    Office = "154 Wednesday drive",
                    EmergencyNumber = "0723096876"
                }, new
                {
                    SupervisorId = 6,
                    ParkId = 6,
                    ParkRef = 6,
                    FirstName = "Daniel",
                    LastName = "Cotting",
                    Salary = (decimal)26350m,
                    StartingDate = new DateTime(2011, 9, 14),
                    Office = "Little Cottage, Ramping Lane",
                    EmergencyNumber = "0787876876"
                }, new
                {
                    SupervisorId = 7,
                    ParkId = 7,
                    ParkRef = 7,
                    FirstName = "Abraham",
                    LastName = "Yoluni",
                    Salary = (decimal)22850m,
                    StartingDate = new DateTime(2008, 2, 22),
                    Office = "Ramping Cottage, Little Lane",
                    EmergencyNumber = "0789871231"
                }
                );

            bd.Entity<SportsClub>()
                .HasData(new
                {
                    ClubId = 1,
                    Name = "Curling Polo Club",
                    ParkId = 1,
                    MembershipFee = (decimal)20000m,
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
                    MembershipFee = (decimal)150m,
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
                    MembershipFee = (decimal)330m,
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
                    OrganiserPhoneNumber = "07123456789",
                    Recurring = "0,6,0"
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
                    OrganiserPhoneNumber = "01196596691",
                    Recurring = "0, 0, 90"
                }) ;
        }
    }
}