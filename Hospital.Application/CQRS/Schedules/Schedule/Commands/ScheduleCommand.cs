using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.CQRS.Schedules.Schedule.Commands
{
    public sealed class ScheduleCommand : IScheduleCommand
    {
        private static HospitalContext _context;
        private static IMapper _mapper;

        public ScheduleCommand(
            HospitalContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ScheduleView> CreateAsync(
            Guid doctorId,
            DayOfWeek dayOfWeek,
            TimeSpan beginWork,
            TimeSpan endWork)
        {
            var doctor = await _context.Doctors
                .Include(i => i.Schedules)
                .FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId));

            if (doctor == null) throw new ArgumentException("Doctor was not found");

            if (doctor.Schedules.FirstOrDefault(i => i.DayOfWeek.Equals(dayOfWeek)) != null)
                throw new ArgumentException("The doctor already has a network schedule for this day");

            var schedule = Domain.Schedule.Create(doctor, dayOfWeek, beginWork, endWork);

            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();

            var result = await _context.Schedules.FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId));

            return _mapper.Map<ScheduleView>(result);
        }

        public async Task<ScheduleView> UpdateAsync(
            Guid scheduleId,
            TimeSpan beginWork,
            TimeSpan endWork)
        {
            var result = await _context.Schedules.FirstOrDefaultAsync(i => i.ScheduleId.Equals(scheduleId));
            if (result == null) throw new ArgumentException("Schedule was not found");
            result.UpdateBeginWork(beginWork);
            result.UpdateEndWork(endWork);
            return _mapper.Map<ScheduleView>(result);
        }

        public async Task DeleteAsync(
            Guid scheduleId)
        {
            var result = await _context.Schedules.FirstOrDefaultAsync(i => i.ScheduleId.Equals(scheduleId));

            _context.Schedules.Remove(result);
            await _context.SaveChangesAsync();
        }
    }
}