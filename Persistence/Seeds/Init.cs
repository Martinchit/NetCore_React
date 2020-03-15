using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Seeds
{
    public class Init
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            ActivitySeed.SeedData(context);
            await UserSeed.SeedData(context, userManager);
        }
    }
}
