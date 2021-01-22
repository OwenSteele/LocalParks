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
            bd.Entity<Park>().HasKey(k => k.ParkId);
            bd.Entity<Supervisor>().HasKey(k => k.SupervisorId);
            bd.Entity<Event>().HasKey(k => k.EventId);
            bd.Entity<SportsClub>().HasKey(k => k.ClubId);

            bd.Entity<Park>().HasOne(p => p.Supervisor).WithOne(s => s.Park)
                .HasForeignKey<Supervisor>(s => s.ParkRef);
            bd.Entity<Park>().HasMany(p => p.SportClubs).WithOne(c => c.Park);
            bd.Entity<Park>().HasMany(p => p.Events).WithOne(e => e.Park);

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
                });

            bd.Entity<Supervisor>()
                .HasData(new
                {
                    SupervisorId = 1,
                    ParkId = 1,
                    ParkRef = 1,
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    Salary = (double)35000,
                    StartingDate = new DateTime(1980, 7, 13)
                });

            bd.Entity<SportsClub>()
                .HasData(new
                {
                    ClubId = 1,
                    Name = "Curling Polo Club",
                    ParkId = 1,
                    MembershipFee = (double)20000,
                    Sport = SportType.Other,
                    Members = 100
                },
                new
                {
                    ClubId = 2,
                    Name = "Meadow RFC",
                    ParkId = 1,
                    MembershipFee = (double)150,
                    Sport = SportType.Rugby,
                    Members = 20
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
                });
        }
    }
}