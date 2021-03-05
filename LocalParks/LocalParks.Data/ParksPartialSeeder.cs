using LocalParks.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using System.IO;
using LocalParks.Data.User;
using Newtonsoft.Json;

namespace LocalParks.Data
{
    public class ParksPartialSeeder
    {
        private readonly ParkContext _context;
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHostingEnvironment _hosting;

        public ParksPartialSeeder(ParkContext context, UserManager<LocalParksUser> userManager,
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


            var user = await _userManager.FindByEmailAsync("contact@owensteele.co.uk");
            if (user == null)
            {
                var file = File.ReadAllText($"{_hosting.ContentRootPath}/Users/adminLocalParksUser.json");
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
        }
    }
}
