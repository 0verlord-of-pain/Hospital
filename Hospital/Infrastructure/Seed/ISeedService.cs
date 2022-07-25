using System.Threading.Tasks;

namespace Hospital.Infrastructure.Seed
{
    internal interface ISeedService
    {
        Task SeedRolesAsync();
        Task SeedAdminAndManagerAsync();
        Task SeedDoctorAndPatientAsync();
        Task SeedRecordsAsync();
    }
}