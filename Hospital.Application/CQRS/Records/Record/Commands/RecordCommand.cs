using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Application.CQRS.Records.Views;
using Hospital.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.CQRS.Records.Record.Commands
{
    public sealed class RecordCommand : IRecordCommand
    {
        private static HospitalContext _context;
        private static IMapper _mapper;

        public RecordCommand(
            HospitalContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RecordView> CreateAsync(Guid userId, Guid doctorId, DateTime bookedTime)
        {
            if (bookedTime <= DateTime.UtcNow)
                throw new ArgumentException("Registration is not available at this time");

            var doctor = await _context.Doctors
                .Include(i => i.Schedules)
                .Include(i => i.Records)
                .FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId));

            if (doctor == null) throw new ArgumentException("Doctor was not found");

            var user = await _context.Users
                .Include(i => i.Records)
                .FirstOrDefaultAsync(i => i.Id.Equals(userId.ToString()));

            if (user == null) throw new ArgumentException("Patient was not found");

            var doctorSchedule = doctor.Schedules
                .FirstOrDefault(i => i.DayOfWeek == bookedTime.DayOfWeek);

            if (doctorSchedule == null)
                throw new ArgumentException("You can't make an appointment with the doctor on this day");

            if (doctorSchedule.BeginWork >= bookedTime.TimeOfDay && doctorSchedule.EndWork <= bookedTime.TimeOfDay)
                throw new ArgumentException("The doctor is not taking at this time");

            if (doctor.Records.FirstOrDefault(i => i.BookedTime.Equals(bookedTime)) != null)
                throw new ArgumentException("Registration already taken");

            await _context.Records.AddAsync(Domain.Record.Create(user, doctor, bookedTime));

            await _context.SaveChangesAsync();

            var result = await _context.Records
                .FirstOrDefaultAsync(i => i.DoctorId.Equals(doctorId) && i.BookedTime.Equals(bookedTime));

            return _mapper.Map<RecordView>(result);
        }

        public async Task DeleteAsync(Guid recordId)
        {
            var record = await _context.Records.FirstOrDefaultAsync(i => i.RecordId.Equals(recordId));
            if (record == null) throw new ArgumentException("Record was not found");

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task<RecordView> UpdateBookedTimeAsync(Guid recordId, DateTime bookedTime)
        {
            if (bookedTime <= DateTime.UtcNow)
                throw new ArgumentException("Registration is not available at this time");

            var doctor = await _context.Doctors
                .Include(i => i.Schedules)
                .FirstOrDefaultAsync(i => i.Records.FirstOrDefault(r => r.RecordId.Equals(recordId)) != null);

            if (doctor == null) throw new ArgumentException("Record was not found");

            var doctorSchedule = doctor.Schedules
                .FirstOrDefault(i => i.DayOfWeek.Equals(bookedTime.DayOfWeek));

            if (doctorSchedule == null)
                throw new ArgumentException("You can't make an appointment with the doctor on this day");

            if (doctorSchedule.BeginWork >= bookedTime.TimeOfDay && doctorSchedule.EndWork <= bookedTime.TimeOfDay)
                throw new ArgumentException("The doctor is not taking at this time");

            var record = await _context.Records.FirstOrDefaultAsync(i => i.RecordId.Equals(recordId));

            record.UpdateBookedTime(bookedTime);
            await _context.SaveChangesAsync();

            return _mapper.Map<RecordView>(record);
        }
    }
}