using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.UserAccounts.Doctors.Commands;
using Hospital.Application.CQRS.UserAccounts.Doctors.Views;
using Hospital.Domain;
using Hospital.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Hospital.Tests.DoctorsTests
{

    public class DoctorCommandTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IDoctorCommand _iDoctorCommand;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public DoctorCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _userManager = MockUserManager.UserManager<User>();
            _iDoctorCommand = new DoctorCommand(_userManager,_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldBeExpected()
        {
            var initUser = _fixture.Build<User>()
                .Create();

            await _hospitalContextFixture.InitUsersAsync(new List<User> { initUser });

            var result = await _iDoctorCommand.CreateAsync(Guid.Parse(initUser.Id), "test");

            result.Should().NotBeNull();
            result.Email.Should().Be(initUser.Email);
            result.Id.Should().Be(initUser.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldBeExpected()
        {
            var doctor = _fixture.Build<Doctor>()
                .Create();

            await _hospitalContextFixture.InitDoctorsAsync(new List<Doctor> { doctor });

            await _iDoctorCommand.DeleteAsync(doctor.DoctorId);

            var result = _hospitalContextFixture.context.Doctors;

            result.Should().NotContain(doctor);
        }

        [Fact]
        public async Task UpdateProfessionAsync_ShouldBeExpected()
        {
            var doctor = _fixture.Build<Doctor>()
                .Create();

            await _hospitalContextFixture.InitDoctorsAsync(new List<Doctor> { doctor });

            var newProfession = "test";

            await _iDoctorCommand.UpdateProfessionAsync(doctor.DoctorId, newProfession);

            var result = await _hospitalContextFixture.context.Doctors.FirstOrDefaultAsync(i=>i.DoctorId.Equals(doctor.DoctorId));

            result.Should().NotBeNull();
            result.Profession.Should().Be(newProfession);
        }
    }
}