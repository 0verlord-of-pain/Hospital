using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Domain;
using Hospital.Storage.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Tests
{
    public class HospitalContextFixture : IDisposable
    {
        public readonly HospitalContext context;

        public HospitalContextFixture()
        {
            var time = DateTime.UtcNow.Millisecond.ToString();
            var options = new DbContextOptionsBuilder<HospitalContext>()
                .UseInMemoryDatabase($"Hospital{time}")
                .Options;

            context = new HospitalContext(options);
        }

        public async Task InitUsersAsync(IEnumerable<User> users)
        {
            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }

        public async Task InitRolesAsync(IEnumerable<IdentityRole> roles)
        {
            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }

        public async Task InitDoctorsAsync(IEnumerable<Doctor> doctors)
        {
            context.Doctors.AddRange(doctors);
            await context.SaveChangesAsync();
        }

        public async Task InitRecordsAsync(IEnumerable<Record> records)
        {
            context.Records.AddRange(records);
            await context.SaveChangesAsync();
        }

        public async Task InitSchedulesAsync(IEnumerable<Schedule> Schedule)
        {
            context.Schedules.AddRange(Schedule);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context?.Dispose();
        }
    }
}