using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.Records.Views;

namespace Hospital.Application.CQRS.Records.Record.Commands
{
    public interface IRecordCommand
    {
        public Task<RecordView> CreateAsync(Guid userId, Guid doctorId, DateTime bookedTime);
        public Task<RecordView> UpdateBookedTimeAsync(Guid recordId, DateTime bookedTime);
        public Task DeleteAsync(Guid recordId);
    }
}