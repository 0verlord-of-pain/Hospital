using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.Schedules.Schedule.Commands;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Domain;
using Moq;
using Xunit;

namespace Hospital.Tests.SchedulesTests
{

    public class ScheduleCommandTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IScheduleCommand _scheduleCommand;
        private readonly IMapper _mapper;

        public ScheduleCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _scheduleCommand = new ScheduleCommand(_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldBeExpected()
        {
            var doctor = _fixture.Build<Doctor>()
                .Without(i=>i.Schedules)
                .Create();

            await _hospitalContextFixture.InitDoctorsAsync(new List<Doctor>{doctor});

            var expectedDayOfWeek = DayOfWeek.Friday;
            var expecteBeginWork = TimeSpan.FromHours(10);
            var expectedEndWork = TimeSpan.FromHours(15);

            var result =  await _scheduleCommand.CreateAsync(
                doctor.DoctorId, expectedDayOfWeek, expecteBeginWork, expectedEndWork);

            result.Should().NotBeNull();
            result.BeginWork.Should().Be(expecteBeginWork.ToString());
            result.EndWork.Should().Be(expectedEndWork.ToString());
            result.DayOfWeek.Should().Be(expectedDayOfWeek.ToString());
        }

        [Fact]
        public async Task UpdateAsync_ShouldBeExpected()
        {
            var schedule = _fixture.Build<Schedule>().Create();

            await _hospitalContextFixture.InitSchedulesAsync(new List<Schedule> { schedule });

            var scheduleId = schedule.ScheduleId;
            var expecteBeginWork = TimeSpan.FromHours(10);
            var expectedEndWork = TimeSpan.FromHours(15);

            var result = await _scheduleCommand.UpdateAsync(
                scheduleId,expecteBeginWork, expectedEndWork);

            result.Should().NotBeNull();
            result.ScheduleId.Should().Be(scheduleId);
            result.BeginWork.Should().Be(expecteBeginWork.ToString());
            result.EndWork.Should().Be(expectedEndWork.ToString());
        }

        [Fact]
        public async Task DeleteAsync_ShouldBeExpected()
        {
            var schedules = _fixture.Build<Schedule>().CreateMany();

            await _hospitalContextFixture.InitSchedulesAsync(schedules);

            var schedule = _hospitalContextFixture.context.Schedules.FirstOrDefault();

            await _scheduleCommand.DeleteAsync(schedule.ScheduleId);

            var result = _hospitalContextFixture.context.Schedules;

            result.Should().NotContain(schedule);
        }
    }
}