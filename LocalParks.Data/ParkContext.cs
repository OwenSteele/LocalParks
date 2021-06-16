using LocalParks.Core.Domain;
using LocalParks.Core.Domain.Shop;
using LocalParks.Core.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<SportType> SportTypes { get; set; }


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
            bd.Entity<Park>().HasMany(p => p.SportClubs).WithOne(c => c.Park)
                .HasForeignKey(c => c.ParkId);
            bd.Entity<Park>().HasMany(p => p.Events).WithOne(e => e.Park)
                .HasForeignKey(e => e.ParkId);
            bd.Entity<LocalParksUser>().HasMany(u => u.OrganisedEvents).WithOne(e => e.User);
            bd.Entity<Order>().HasMany(o => o.Items).WithOne(i => i.Order);
            bd.Entity<OrderItem>().HasOne(i => i.Product);
            bd.Entity<SportsClub>().HasOne(i => i.Sport);

            // defined keys
            bd.Entity<Postcode>().HasKey(k => k.Zone);
            bd.Entity<Park>().HasKey(k => k.ParkId);
            bd.Entity<Supervisor>().HasKey(k => k.SupervisorId);
            bd.Entity<SportsClub>().HasKey(k => k.ClubId);
            bd.Entity<ParkEvent>().HasKey(k => k.EventId);
            bd.Entity<Product>().HasKey(k => k.ProductId);
            bd.Entity<Order>().HasKey(k => k.OrderId);
            bd.Entity<OrderItem>().HasKey(k => k.ItemId);
            bd.Entity<SportType>().HasKey(k => k.SportTypeId);

            // decimal properties
            bd.Entity<SportsClub>().Property(c => c.MembershipFee)
                .HasColumnType("decimal(18,2)");
            bd.Entity<Supervisor>().Property(s => s.Salary)
                .HasColumnType("decimal(18,2)");
            bd.Entity<Park>().Property(p => p.Latitude)
                .HasColumnType("decimal(18,2)");
            bd.Entity<Park>().Property(p => p.Longitude)
                .HasColumnType("decimal(18,2)");

            bd.Entity<Product>().Property(p => p.Price)
                .HasColumnType("decimal(18,2)");
            bd.Entity<OrderItem>().Property(p => p.UnitPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}