using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.Schedules.Views;

namespace Hospital.Application.CQRS.Schedules.Schedule.Commands
{
    public interface IScheduleCommand
    {
        public Task<ScheduleView> CreateAsync(
            Guid doctorId,
            DayOfWeek dayOfWeek,
            TimeSpan beginWork,
            TimeSpan endWork);

        public Task<ScheduleView> UpdateAsync(
            Guid scheduleId,
            TimeSpan beginWork,
            TimeSpan endWork);

        public Task DeleteAsync(
            Guid scheduleId);
    }
}