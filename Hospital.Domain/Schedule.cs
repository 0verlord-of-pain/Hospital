using System;
using Hospital.Domain.Extensions;

namespace Hospital.Domain
{
    public class Schedule : ICreatedOnUtc, IUpdatedOnUtc
    {
        public Guid ScheduleId { get; set; }
        public Doctor Doctor { get; set; }
        public Guid DoctorId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan BeginWork { get; set; }
        public TimeSpan EndWork { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public static Schedule Create(
            Doctor doctor,
            DayOfWeek dayOfWeek,
            TimeSpan beginWork,
            TimeSpan endWork)
        {
            return new Schedule
            {
                Doctor = doctor,
                DoctorId = doctor.DoctorId,
                DayOfWeek = dayOfWeek,
                BeginWork = beginWork,
                EndWork = endWork
            };
        }

        public Schedule UpdateBeginWork(TimeSpan beginWork)
        {
            BeginWork = beginWork;
            return this;
        }

        public Schedule UpdateEndWork(TimeSpan endWork)
        {
            EndWork = endWork;
            return this;
        }
    }
}