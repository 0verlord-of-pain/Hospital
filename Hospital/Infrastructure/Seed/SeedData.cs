using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.Seed
{
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var seedService = scope.ServiceProvider.GetRequiredService<ISeedService>();
            await seedService.SeedRolesAsync();
            await seedService.SeedAdminAndManagerAsync();
            await seedService.SeedDoctorAndPatientAsync();
            await seedService.SeedRecordsAsync();
        }
    }
}