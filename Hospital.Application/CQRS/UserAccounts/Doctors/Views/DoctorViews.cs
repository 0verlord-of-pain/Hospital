using System.Collections.Generic;
using Hospital.Application.CQRS.Records.Views;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Application.CQRS.UserAccounts.Views;

namespace Hospital.Application.CQRS.UserAccounts.Doctors.Views
{
    public class DoctorViews : UserAccountView
    {
        public string Profession { get; set; }
        public IEnumerable<ScheduleView> Schedules { get; set; }
        public IEnumerable<RecordView> Records { get; set; }
    }
}