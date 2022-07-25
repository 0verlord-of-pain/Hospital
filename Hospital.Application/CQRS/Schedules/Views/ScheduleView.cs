using System;

namespace Hospital.Application.CQRS.Schedules.Views
{
    public class ScheduleView
    {
        public Guid ScheduleId { get; set; }
        public Guid DoctorId { get; set; }
        public string DayOfWeek { get; set; }
        public string BeginWork { get; set; }
        public string EndWork { get; set; }
    }
}