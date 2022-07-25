using System;
using System.Collections.Generic;
using Hospital.Domain.Extensions;

namespace Hospital.Domain
{
    public class Doctor : ICreatedOnUtc, IUpdatedOnUtc
    {
        public Guid DoctorId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string Profession { get; set; }
        public List<Schedule> Schedules { get; set; }
        public List<Record> Records { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public static Doctor Create(User user, string profession)
        {
            return new Doctor
            {
                User = user,
                DoctorId = Guid.Parse(user.Id),
                Profession = profession
            };
        }

        public Doctor UpdateProfession(string profession)
        {
            Profession = profession;
            return this;
        }

        public Doctor UpdateSchedules(List<Schedule> schedules)
        {
            Schedules = schedules;
            return this;
        }
    }
}