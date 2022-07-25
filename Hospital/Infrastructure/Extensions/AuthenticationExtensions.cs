using Hospital.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Infrastructure.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationCustom(this IServiceCollection services)
        {
            services
                .AddAuthorizationCore(options =>
                {
                    options.AddPolicy(Policies.User,
                        policy => policy.RequireAuthenticatedUser());
                    options.AddPolicy(Policies.Doctor,
                        policy => policy.RequireRole(Policies.Doctor));
                    options.AddPolicy(Policies.Admin,
                        policy => policy.RequireRole(Policies.Admin));
                    options.AddPolicy(Policies.Manager,
                        policy => policy.RequireRole(Policies.Manager));
                    options.AddPolicy(Policies.AdminOrManager,
                        policy => policy.RequireRole(Policies.Manager, Policies.Admin));
                    options.AddPolicy(Policies.AdminOrManagerOrDoctor,
                        policy => policy.RequireRole(Policies.Manager, Policies.Admin, Policies.Doctor));
                });

            return services;
        }
    }
}