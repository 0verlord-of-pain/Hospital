using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.Domain;
using Hospital.Domain.Enums;
using Hospital.Storage.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Infrastructure.Seed
{
    internal sealed class SeedService : ISeedService
    {
        private readonly HospitalContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public SeedService(
            HospitalContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedRolesAsync()
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Doctor.ToString()));
        }

        public async Task SeedAdminAndManagerAsync()
        {
            var defaultAdmin = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (_userManager.Users.All(u => u.Id != defaultAdmin.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultAdmin.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultAdmin, "admin1");
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.Manager.ToString());
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.User.ToString());
                    await _userManager.AddToRoleAsync(defaultAdmin, Roles.Doctor.ToString());
                }
            }

            var defaultManager = new User
            {
                UserName = "manager",
                Email = "manager@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (_userManager.Users.All(u => u.Id != defaultManager.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultManager.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultManager, "manager");
                    await _userManager.AddToRoleAsync(defaultManager, Roles.Manager.ToString());
                    await _userManager.AddToRoleAsync(defaultManager, Roles.Doctor.ToString());
                    await _userManager.AddToRoleAsync(defaultManager, Roles.User.ToString());
                }
            }
        }

        public async Task SeedDoctorAndPatientAsync()
        {
            var defaultDoctorInfo = new User
            {
                Surname = "Ivanov",
                UserName = "doctor1@gmail.com",
                Name = "Ivan",
                Email = "doctor1@gmail.com",
                PhoneNumber = "380599952358",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (_userManager.Users.All(u => u.Id != defaultDoctorInfo.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultDoctorInfo.Email);
                if (user == null)
                {
                    await _userManager.AddToRoleAsync(defaultDoctorInfo, Roles.User.ToString());
                    await _userManager.AddToRoleAsync(defaultDoctorInfo, Roles.Doctor.ToString());
                }
            }

            defaultDoctorInfo = await _userManager.FindByEmailAsync(defaultDoctorInfo.Email);
            var defaultDoctor = Doctor.Create(defaultDoctorInfo, "Dentist");

            if (!_context.Doctors.Any())
            {
                _context.Doctors.Add(defaultDoctor);
                await _context.SaveChangesAsync();
                var doctorSchedules = new List<Schedule>
                {
                    Schedule.Create(defaultDoctor, DayOfWeek.Monday, TimeSpan.FromHours(10), TimeSpan.FromHours(15)),
                    Schedule.Create(defaultDoctor, DayOfWeek.Tuesday, TimeSpan.FromHours(10), TimeSpan.FromHours(15)),
                    Schedule.Create(defaultDoctor, DayOfWeek.Wednesday, TimeSpan.FromHours(10), TimeSpan.FromHours(15))
                };

                _context.Schedules.AddRange(doctorSchedules);
                await _context.SaveChangesAsync();

                var doctor = await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorId.Equals(defaultDoctor.DoctorId));

                doctor?.UpdateSchedules(doctorSchedules);
            }


            var defaultPatient = new User
            {
                Surname = "Zubenko",
                Name = "Mehail",
                UserName = "zubenko@gmail.com",
                Email = "zubenko@gmail.com",
                PhoneNumber = "380639952358",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (_userManager.Users.All(u => u.Id != defaultPatient.Id))
            {
                var user = await _userManager.FindByEmailAsync(defaultPatient.Email);
                if (user == null)
                {
                    await _userManager.CreateAsync(defaultPatient, "zubenko");
                    await _userManager.AddToRoleAsync(defaultPatient, Roles.User.ToString());
                }
            }
        }

        public async Task SeedRecordsAsync()
        {
            if (!_context.Records.Any())
            {
                var patient = await _context.Users.FirstOrDefaultAsync(i => i.Email.Equals("zubenko@gmail.com"));
                var doctorInfo = await _context.Users.FirstOrDefaultAsync(i => i.Email.Equals("doctor1@gmail.com"));
                var doctor =
                    await _context.Doctors.FirstOrDefaultAsync(i => i.DoctorId.Equals(Guid.Parse(doctorInfo.Id)));
                if (patient != null && doctor != null)
                {
                    var temp = DateTime.UtcNow;
                    var recordTime = new DateTime(temp.Year, temp.Month, temp.Day, 10, 0, 0);
                    _context.Records.Add(
                        Record.Create(patient, doctor, recordTime));

                    var a = await _context.Records.ToListAsync();
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}