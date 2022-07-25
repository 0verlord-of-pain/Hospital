using Hospital.Storage.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.Extensions
{
    public static class DbConfigurationExtensions
    {
        public static IServiceCollection AddDbContextsCustom(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DefaultConnection");

            services
                .AddDbContext<HospitalContext>(options => options.UseSqlServer(connection,
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(HospitalContext).Assembly.FullName)));

            return services;
        }
    }
}