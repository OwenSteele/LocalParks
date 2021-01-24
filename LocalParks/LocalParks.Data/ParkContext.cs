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
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(_config.GetConnectionString("LocalParks"));
        }

        protected override void OnModelCreating(ModelBuilder bd)
        {  

            //defined relationships
            bd.Entity<Park>().HasOne(p => p.Supervisor).WithOne(s => s.Park)
                .HasForeignKey<Supervisor>(s => s.ParkRef);
            bd.Entity<Park>().HasMany(p => p.SportClubs).WithOne(c => c.Park);
            bd.Entity<Park>().HasMany(p => p.Events).WithOne(e => e.Park);

            //defined keys
            bd.Entity<Park>().HasKey(k => k.ParkId);
            bd.Entity<Supervisor>().HasKey(k => k.SupervisorId);
            bd.Entity<SportsClub>().HasKey(k => k.ClubId);
            bd.Entity<Event>().HasKey(k => k.EventId);

            //defined starting data
            bd.Entity<Park>()
                .HasData(new
                {
                    ParkId = 1,
                    Postcode = PostcodeType.LP4.ToString(),
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
                    Postcode = PostcodeType.PF15.ToString(),
                    Name = "Middlesbrook Park",
                    SizeInMetresSquared = 85000,
                    Longitude = 38.8765,
                    Latitude = 0.18798,
                    OpeningTime = new DateTime(2021, 1, 1, 6, 0, 0),
                    ClosingTime = new DateTime(2021, 1, 1, 22, 0, 0),
                    SupervisorId = 2
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

            bd.Entity<Event>()
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