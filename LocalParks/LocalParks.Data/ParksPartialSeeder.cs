using LocalParks.Core;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace LocalParks.Data
{
    public class ParksPartialSeeder
    {
        private readonly ParkContext _context;
        private readonly UserManager<LocalParksUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ParksPartialSeeder(ParkContext context, UserManager<LocalParksUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("contact@owensteele.co.uk");
            if (user == null)
            {
                user = new LocalParksUser
                {
                    UserName = "Owen",
                    FirstName = "Owen",
                    LastName = "Steele",
                    Email = "contact@owensteele.co.uk",
                    PhoneNumber = "070000000000",
                    PostcodeZone = "LP1",
                    MemberSince = new DateTime(2021, 2, 1)
                };

                var result = await _userManager.CreateAsync(user, "AdminP4ssw0rd!.");
                if (!result.Succeeded) throw new Exception("Could not seed user.");

                _context.SaveChanges();
            }

            if (user.MemberSince == DateTime.MinValue)
            {
                user.MemberSince = new DateTime(2021, 2, 1);
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) throw new Exception("Could not seed member since");

                _context.SaveChanges();
            }

            var test = await _userManager.FindByEmailAsync("example@owensteele.co.uk");
            if (test == null || user.MemberSince == DateTime.MinValue)
            {
                test = new LocalParksUser
                {
                    UserName = "TestUser",
                    FirstName = "Test",
                    LastName = "User",
                    Email = "example@owensteele.co.uk",
                    PhoneNumber = "099999999999",
                    PostcodeZone = "LP1",
                    MemberSince = DateTime.Today
                };

                var result = await _userManager.CreateAsync(test, "Test12345678!");
                if (!result.Succeeded) throw new Exception("Could not seed user.");

                _context.SaveChanges();
            }

            if (test.MemberSince == DateTime.MinValue)
            {
                test.MemberSince = new DateTime(2021, 2, 1);
                var result = await _userManager.UpdateAsync(test);
                if (!result.Succeeded) throw new Exception("Could not seed member since");

                _context.SaveChanges();
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

                _context.SaveChanges();
            }

            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                var result = await _userManager.AddToRoleAsync(user, "Administrator");
                if (!result.Succeeded) throw new Exception("Could not seed User with Role.");
            }

            _context.SaveChanges();
        }
    }
}
