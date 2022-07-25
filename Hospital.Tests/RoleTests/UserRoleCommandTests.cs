using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.UserAccounts.Role.Commands;
using Hospital.Application.CQRS.UserAccounts.Role.Views;
using Hospital.Domain;
using Hospital.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Hospital.Tests.RoleTests
{
    public class UserRoleCommandTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IUserRoleCommand _iUserRoleCommand;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserRoleCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _userManager = MockUserManager.UserManager<User>();

            _iUserRoleCommand = new UserRoleCommand(_userManager, _hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task AddUserRoleAsync_ShouldBeExpected()
        {
            var attachRole = Roles.Admin.ToString();
            var initUser = _fixture.Build<User>()
                .Create();

            await _hospitalContextFixture.InitUsersAsync(new List<User> { initUser });

            var user = await _iUserRoleCommand.AddUserRoleAsync(Guid.Parse(initUser.Id), attachRole);

            user.Should().NotBeNull();
            user.Email.Should().Be(initUser.Email);
            user.Id.Should().Be(initUser.Id);
        }

        [Fact]
        public async Task RemoveUserRoleAsync_ShouldBeExpected()
        {
            var removeRole = Roles.Admin.ToString();
            var initUser = _fixture.Build<User>()
                .Create();

            await _hospitalContextFixture.InitUsersAsync(new List<User> { initUser });

            var user = await _iUserRoleCommand.RemoveUserRoleAsync(Guid.Parse(initUser.Id), removeRole);

            user.Should().NotBeNull();
            user.Email.Should().Be(initUser.Email);
            user.Id.Should().Be(initUser.Id);
        }
    }
}