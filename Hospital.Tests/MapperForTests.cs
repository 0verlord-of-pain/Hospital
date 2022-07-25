using AutoMapper;
using Hospital.Application.CQRS.Records.Views;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;
using Hospital.Application.CQRS.UserAccounts.Role.Views;
using Hospital.Application.CQRS.UserAccounts.Views;
using Hospital.Domain;

namespace Hospital.Tests
{
    public class MapperForTests
    {
        public static MapperConfiguration GetConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Doctor, DoctorViews>()
                    .ForMember(dest => dest.Id,
                        dest => dest
                            .MapFrom(src => src.DoctorId))
                    .ForMember(dest => dest.Email,
                        dest => dest
                            .MapFrom(src => src.User.Email))
                    .ForMember(dest => dest.PhoneNumber,
                        dest => dest
                            .MapFrom(src => src.User.PhoneNumber))
                    .ForMember(dest => dest.Surname,
                        dest => dest
                            .MapFrom(src => src.User.Surname))
                    .ForMember(dest => dest.Name,
                        dest => dest
                            .MapFrom(src => src.User.Name));
                cfg.CreateMap<User, UserAccountView>();
                cfg.CreateMap<Schedule, ScheduleView>()
                    .ForMember(dest => dest.DayOfWeek,
                        dest => dest
                            .MapFrom(src => src.DayOfWeek.ToString()))
                    .ForMember(dest => dest.BeginWork,
                        dest => dest
                            .MapFrom(src => src.BeginWork.ToString()))
                    .ForMember(dest => dest.EndWork,
                        dest => dest
                            .MapFrom(src => src.EndWork.ToString()));

                cfg.CreateMap<Record, RecordView>();

                cfg.CreateMap<User, UserRoleView>()
                    .ForMember(dest => dest.Roles, src => src.Ignore());
            });

            return config;
        }
    }
}