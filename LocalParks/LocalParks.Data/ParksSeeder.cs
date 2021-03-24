using LocalParks.Core;
using LocalParks.Core.Shop;
using LocalParks.Core.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Data
{
    public class ParksSeeder
    {
        private readonly ParkContext _context;
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hosting;

        public ParksSeeder(ParkContext context, UserManager<LocalParksUser> userManager,
            RoleManager<IdentityRole> roleManager, IHostingEnvironment hosting)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _hosting = hosting;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            var folderPath = $"{_hosting.ContentRootPath}/seededData";

            var user = await _userManager.FindByEmailAsync("contact@owensteele.co.uk");
            if (user == null)
            {
                var filePath = Path.Combine(folderPath, "adminLocalParksUser.json");
                var file = File.ReadAllText(filePath);
                var seededUser = JsonConvert.DeserializeObject<LocalParksSeedUser>(file);

                user = new LocalParksUser
                {
                    UserName = seededUser.UserName,
                    FirstName = seededUser.FirstName,
                    LastName = seededUser.LastName,
                    Email = seededUser.Email,
                    PhoneNumber = seededUser.PhoneNumber,
                    PostcodeZone = seededUser.PostcodeZone,
                    MemberSince = new DateTime(2021, 2, 1)
                };

                var result = await _userManager.CreateAsync(user, seededUser.Password);
                if (!result.Succeeded) throw new Exception("Could not seed user.");

                await _context.SaveChangesAsync();
            }

            if (user.MemberSince == DateTime.MinValue)
            {
                user.MemberSince = new DateTime(2021, 2, 1);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) throw new Exception("Could not seed member since");

                await _context.SaveChangesAsync();
            }

            var roleAdmin = await _roleManager.FindByNameAsync("Administrator");
            if (roleAdmin == null)
            {
                roleAdmin = new IdentityRole
                {
                    Id = "Administrator",
                    Name = "Administrator",
                };

                var result = await _roleManager.CreateAsync(roleAdmin);
                if (!result.Succeeded) throw new Exception("Could not seed Admin role.");

                await _context.SaveChangesAsync();
            }

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                var result = await _userManager.AddToRoleAsync(user, "Administrator");
                if (!result.Succeeded) throw new Exception("Could not seed User with Role.");
            }

            await _context.SaveChangesAsync();

            //
            // seed postcodes
            //                    

            if (!_context.Postcodes.Any())
            {
                var filePath = Path.Combine(folderPath, "postcodes.json");
                var file = File.ReadAllText(filePath);
                var seeded = JsonConvert.DeserializeObject<IEnumerable<Postcode>>(file);

                await _context.Postcodes.AddRangeAsync(seeded);

                await _context.SaveChangesAsync();
            }

            // seed parks              

            if (!_context.Parks.Any())
            {
                var filePath = Path.Combine(folderPath, "parks.json");
                var file = File.ReadAllText(filePath);
                var seeded = JsonConvert.DeserializeObject<IEnumerable<Park>>(file);

                await _context.Parks.AddRangeAsync(seeded);

                await _context.SaveChangesAsync();
            }

            // seed supervisors              

            if (!_context.Supervisors.Any())
            {
                var filePath = Path.Combine(folderPath, "supervisors.json");
                var file = File.ReadAllText(filePath);
                var seeded = JsonConvert.DeserializeObject<IEnumerable<Supervisor>>(file);

                await _context.Supervisors.AddRangeAsync(seeded);

                await _context.SaveChangesAsync();
            }

            // seed park events

            if (!_context.Events.Any())
            {
                var filePath = Path.Combine(folderPath, "parkEvents.json");
                var file = File.ReadAllText(filePath);
                var seeded = JsonConvert.DeserializeObject<IEnumerable<ParkEvent>>(file);

                await _context.Events.AddRangeAsync(seeded);

                await _context.SaveChangesAsync();
            }

            // seed shop products BEFORE SPORTS CLUBS

            if (!_context.Products.Any())
            {
                var filePath = Path.Combine(folderPath, "products.json");
                var file = File.ReadAllText(filePath);
                var seeded = JsonConvert.DeserializeObject<IEnumerable<Product>>(file);

                await _context.Products.AddRangeAsync(seeded);

                await _context.SaveChangesAsync();
            }

            // seed sports clubs                

            if (!_context.SportsClubs.Any())
            {
                var filePath = Path.Combine(folderPath, "sportsClubs.json");
                var file = File.ReadAllText(filePath);
                var seeded = JsonConvert.DeserializeObject<IEnumerable<SportsClub>>(file);

                await _context.SportsClubs.AddRangeAsync(seeded);

                foreach (var club in seeded)
                {
                    var membershipProduct = new Product
                    {
                        Name = $"{club.Name} membership",
                        Price = club.MembershipFee,
                        Description = $"Membership for Sports Club: {club.Name}",
                        ImageId = "membership_1",
                        Category = ProductCategoryType.ClubMembership
                    };

                    await _context.Products.AddAsync(membershipProduct);
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
