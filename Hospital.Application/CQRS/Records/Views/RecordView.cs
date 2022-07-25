using System;

namespace Hospital.Application.CQRS.Records.Views
{
    public class RecordView
    {
        public Guid RecordId { get; set; }
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime BookedTime { get; set; }
    }
}