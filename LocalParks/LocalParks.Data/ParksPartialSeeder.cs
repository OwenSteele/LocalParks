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

        public ParksPartialSeeder(ParkContext context, UserManager<LocalParksUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            var user = await GetUserToSeedAsync("contact@owensteele.co.uk");
            if (user == null) return;

            var result = await _userManager.CreateAsync(user, "AdminP4ssw0rd!.");
            if (result != IdentityResult.Success) throw new Exception("Could not seed user.");

            _context.SaveChanges();
        }

        public async Task<LocalParksUser> GetUserToSeedAsync(string email = null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new LocalParksUser
                {
                    UserName = "Owen",
                    FirstName = "Owen",
                    LastName = "Steele",
                    Email = "contact@owensteele.co.uk",
                    PhoneNumber = "070000000000",
                    PostcodeZone = "LP1"
                };
                return user;
            }
            return null;
        }
    }
}
