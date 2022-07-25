using System.Threading;
using System.Threading.Tasks;
using Hospital.Domain;
using Hospital.Storage.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Storage.Persistence
{
    public class HospitalContext : IdentityDbContext<User>
    {
        public HospitalContext(
            DbContextOptions<HospitalContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.UpdateSystemDates();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}