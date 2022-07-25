using System;
using System.Collections.Generic;
using Hospital.Application.CQRS.Schedules.Views;

namespace Hospital.Application.CQRS.Schedules.Schedule.Queries
{
    public interface IScheduleQuery
    {
        public IEnumerable<ScheduleView> GetAll();
        public IEnumerable<ScheduleView> GetByDoctorId(Guid doctorId);
    }
}