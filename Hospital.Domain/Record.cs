using System;
using Hospital.Domain.Extensions;

namespace Hospital.Domain
{
    public class Record : ICreatedOnUtc, IUpdatedOnUtc
    {
        public Guid RecordId { get; set; }
        public User Patient { get; set; }
        public Guid PatientId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime BookedTime { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public static Record Create(
            User patient,
            Doctor doctor,
            DateTime bookedTime)
        {
            return new Record
            {
                Patient = patient,
                PatientId = Guid.Parse(patient.Id),
                Doctor = doctor,
                DoctorId = doctor.DoctorId,
                BookedTime = bookedTime
            };
        }

        public Record UpdateBookedTime(DateTime bookedTime)
        {
            BookedTime = bookedTime;
            return this;
        }
    }
}