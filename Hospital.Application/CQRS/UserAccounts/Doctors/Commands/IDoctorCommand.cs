using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;

namespace Hospital.Application.CQRS.UserAccounts.Doctors.Commands
{
    public interface IDoctorCommand
    {
        public Task<DoctorViews> CreateAsync(Guid userId, string profession);
        public Task<DoctorViews> UpdateProfessionAsync(Guid doctorId, string profession);
        public Task DeleteAsync(Guid doctorId);
    }
}