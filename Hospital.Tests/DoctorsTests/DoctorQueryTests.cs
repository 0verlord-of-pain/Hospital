using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.Schedules.Schedule.Queries;
using Hospital.Application.CQRS.UserAccounts.Doctors.Queries;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;
using Hospital.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Hospital.Tests.DoctorsTests
{
    public class DoctorQueryTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IDoctorQuery _iDoctorQuery;
        private readonly IMapper _mapper;

        public DoctorQueryTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _iDoctorQuery = new DoctorQuery(_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetDoctors_ShouldBeExpected()
        {
            var doctors = _fixture.Build<Doctor>().CreateMany();

            await _hospitalContextFixture.InitDoctorsAsync(doctors);

            var result = _iDoctorQuery.GetDoctors();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ShouldBeExpected()
        {
            var doctors = _fixture.Build<Doctor>().CreateMany();
            await _hospitalContextFixture.InitDoctorsAsync(doctors);

            var doctorId = doctors.Select(i=>i.DoctorId).FirstOrDefault();;

            var result = await _iDoctorQuery.GetDoctorByIdAsync(doctorId);

            result.Should().NotBeNull();
            result.Id.Should().Be(doctorId);
        }
    }
}