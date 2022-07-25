using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;

namespace Hospital.Application.CQRS.UserAccounts.Doctors.Queries
{
    public interface IDoctorQuery
    {
        public IEnumerable<DoctorViews> GetDoctors();
        public Task<DoctorViews> GetDoctorByIdAsync(Guid doctorId);
    }
}