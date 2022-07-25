using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Hospital.Application.CQRS.Records.Views;
using Hospital.Storage.Persistence;

namespace Hospital.Application.CQRS.Records.Record.Queries
{
    public sealed class RecordQuery : IRecordQuery
    {
        private static HospitalContext _context;
        private static IMapper _mapper;

        public RecordQuery(
            HospitalContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<RecordView> GetAll()
        {
            var result = _context.Records;

            return _mapper.Map<IEnumerable<RecordView>>(result);
        }

        public IEnumerable<RecordView> GetByDoctorId(Guid doctorId)
        {
            var result = _context.Records.Where(i => i.DoctorId.Equals(doctorId));
            if (!result.Any()) throw new ArgumentException("Doctor was not found");
            return _mapper.Map<IEnumerable<RecordView>>(result);
        }

        public IEnumerable<RecordView> GetByUserId(Guid userId)
        {
            var result = _context.Records.Where(i => i.PatientId.Equals(userId));
            if (!result.Any()) throw new ArgumentException("Patient was not found");
            return _mapper.Map<IEnumerable<RecordView>>(result);
        }
    }
}