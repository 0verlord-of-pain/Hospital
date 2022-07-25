using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Application.CQRS.Records.Views;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;
using Hospital.Application.Extensions;
using Hospital.Domain;
using Hospital.Domain.Enums;
using Hospital.Storage.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.CQRS.UserAccounts.Doctors.Commands
{
    public sealed class DoctorCommand : IDoctorCommand
    {
        private static UserManager<User> _userManager;
        private static HospitalContext _context;
        private static IMapper _mapper;

        public DoctorCommand(
            UserManager<User> userManager,
            HospitalContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<DoctorViews> CreateAsync(Guid userId, string profession)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.Id.Equals(userId.ToString()));

            var checkDoctor = await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorId.Equals(userId));

            if (checkDoctor != null)
            {
                throw new ArgumentException("Doctor was found");
            }

            if (user == null)
            {
                throw new ArgumentException("User was not found");
            }

            if (string.IsNullOrWhiteSpace(profession))
            {
                throw new ArgumentException("Invalid profession value");
            }

            var newDoctor = Doctor.Create(user, profession);

            _context.Doctors.Add(newDoctor);

            var result = await _userManager.AddToRoleAsync(user, Roles.Doctor.ToString());

            if (result.TryGetErrors(out var error))
            {
                throw new ArgumentException(error);
            }

            await _context.SaveChangesAsync();

            var doctor = await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorId.Equals(userId));

            if (doctor == null)
            {
                throw new ArgumentException("Doctor was not found");
            }

            return _mapper.Map<DoctorViews>(doctor);
        }

        public async Task DeleteAsync(Guid doctorId)
        {
            var doctor = await _context.Doctors
                .Include(i=>i.User)
                .FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId));

            if (doctor == null) throw new ArgumentException("Doctor was not found");

            var result = await _userManager.RemoveFromRoleAsync(doctor.User, Roles.Doctor.ToString());

            if (result.TryGetErrors(out var error))
            {
                throw new ArgumentException(error);
            }

            _context.Remove(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task<DoctorViews> UpdateProfessionAsync(Guid doctorId, string profession)
        {
            var result = await _context.Doctors
                .Include(i => i.Schedules)
                .Include(i => i.Records)
                .Include(i => i.User)
                .FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId));

            if (result == null) throw new ArgumentException("Doctor was not found");

            if (string.IsNullOrWhiteSpace(profession)) throw new ArgumentException("Invalid profession value");

            result.UpdateProfession(profession);
            await _context.SaveChangesAsync();

            var doctor = _mapper.Map<DoctorViews>(result);

            doctor.Records = _mapper.Map<IEnumerable<RecordView>>(result.Records);
            doctor.Schedules = _mapper.Map<IEnumerable<ScheduleView>>(result.Schedules);

            return doctor;
        }
    }
}