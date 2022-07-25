using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.Schedules.Schedule.Queries;
using Hospital.Application.CQRS.Schedules.Views;
using Hospital.Domain;
using Moq;
using Xunit;

namespace Hospital.Tests.SchedulesTests
{
    public class ScheduleQueryTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IScheduleQuery _iScheduleQuery;
        private readonly IMapper _mapper;

        public ScheduleQueryTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _iScheduleQuery = new ScheduleQuery(_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetAll_ShouldBeExpected()
        {
            var schedules = _fixture.Build<Schedule>().CreateMany();

            await _hospitalContextFixture.InitSchedulesAsync(schedules);

            var result = _iScheduleQuery.GetAll();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByDoctorId_ShouldBeExpected()
        {
            var schedules = _fixture.Build<Schedule>().CreateMany();

            await _hospitalContextFixture.InitSchedulesAsync(schedules);

            var doctorId = schedules.Select(i => i.DoctorId).FirstOrDefault();
            var scheduleCount = schedules.Count(i => i.DoctorId.Equals(doctorId));

            var result = _iScheduleQuery.GetByDoctorId(doctorId);

            result.Should().NotBeNull();
            result.Count().Should().Be(scheduleCount);
        }
    }
}