using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Storage.Persistence;

namespace Hospital.Application.CQRS.Schedules.Schedule.Queries
{
    public sealed class ScheduleQuery : IScheduleQuery
    {
        private static HospitalContext _context;
        private static IMapper _mapper;

        public ScheduleQuery(
            HospitalContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<ScheduleView> GetAll()
        {
            var result = _context.Schedules;

            return _mapper.Map<IEnumerable<ScheduleView>>(result);
        }

        public IEnumerable<ScheduleView> GetByDoctorId(Guid doctorId)
        {
            var result = _context.Schedules.Where(i => i.DoctorId.Equals(doctorId));
            return _mapper.Map<IEnumerable<ScheduleView>>(result);
        }
    }
}