using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;
using Hospital.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.CQRS.UserAccounts.Doctors.Queries
{
    public sealed class DoctorQuery : IDoctorQuery
    {
        private static HospitalContext _context;
        private static IMapper _mapper;

        public DoctorQuery(
            HospitalContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DoctorViews> GetDoctorByIdAsync(Guid doctorId)
        {
            var result = await _context.Doctors
                .Include(i => i.User)
                .Include(i => i.Schedules)
                .Include(i => i.Records)
                .FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId));

            return _mapper.Map<DoctorViews>(result);
        }

        public IEnumerable<DoctorViews> GetDoctors()
        {
            var result = _context.Doctors
                .Include(i => i.User)
                .Include(i => i.Schedules)
                .Include(i => i.Records);

            return _mapper.Map<IEnumerable<DoctorViews>>(result);
        }
    }
}