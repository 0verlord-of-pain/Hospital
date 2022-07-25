using System;
using System.Reflection;
using Hospital.Application.CQRS.Records.Record.Commands;
using Hospital.Application.CQRS.Records.Record.Queries;
using Hospital.Application.CQRS.Schedules.Schedule.Commands;
using Hospital.Application.CQRS.Schedules.Schedule.Queries;
using Hospital.Application.CQRS.UserAccounts.Doctors.Commands;
using Hospital.Application.CQRS.UserAccounts.Doctors.Queries;
using Hospital.Application.CQRS.UserAccounts.Role.Commands;
using Hospital.Application.CQRS.UserAccounts.Role.Queries;
using Hospital.Application.CQRS.UserAccounts.Users.Queries;
using Hospital.Application.Mapper;
using Hospital.Domain;
using Hospital.Infrastructure.Extensions;
using Hospital.Infrastructure.Seed;
using Hospital.Storage.Persistence;
using Hospital.Tests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextsCustom(Configuration);
            services.AddControllers();
            services
                .AddIdentity<User, IdentityRole>(ops =>
                {
                    ops.SignIn.RequireConfirmedEmail = false;
                    ops.User.RequireUniqueEmail = true;
                    ops.Password.RequireDigit = false;
                    ops.Password.RequireUppercase = false;
                    ops.Password.RequireNonAlphanumeric = false;
                    ops.Password.RequiredLength = 5;
                    ops.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<HospitalContext>();

            services.AddScoped<ISeedService, SeedService>();
            services.AddTransient<IUserAccountQuery, UserAccountQuery>();
            services.AddTransient<IScheduleQuery, ScheduleQuery>();
            services.AddTransient<IScheduleCommand, ScheduleCommand>();
            services.AddTransient<IRecordCommand, RecordCommand>();
            services.AddTransient<IRecordQuery, RecordQuery>();
            services.AddTransient<IDoctorQuery, DoctorQuery>();
            services.AddTransient<IDoctorCommand, DoctorCommand>();
            services.AddTransient<IUserRoleCommand, UserRoleCommand>();
            services.AddTransient<IUserRoleQuery, UserRoleQuery>();

            services.AddAuthenticationCustom();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(RegisterViews).GetTypeInfo().Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseErrorHandler(env);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}