using System;
namespace Persistence.Seeds
{
    public class Init
    {
        public static void SeedData(DataContext context)
        {
            ActivitySeed.SeedData(context);
        }
    }
}
