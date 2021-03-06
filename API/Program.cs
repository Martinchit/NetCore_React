using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Domain;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) 
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                    Init.SeedData(context, userManager).Wait();
                    logger.LogInformation("Files migrated");
                }
                catch(Exception ex) 
                {
                    logger.LogError(ex, "Error occurred during database migration");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
