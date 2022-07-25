using System;
using System.Collections.Generic;
using Hospital.Application.CQRS.Records.Views;

namespace Hospital.Application.CQRS.Records.Record.Queries
{
    public interface IRecordQuery
    {
        public IEnumerable<RecordView> GetAll();
        public IEnumerable<RecordView> GetByDoctorId(Guid doctorId);
        public IEnumerable<RecordView> GetByUserId(Guid userId);
    }
}