using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.Schedules.Schedule.Queries;
using Hospital.Application.CQRS.UserAccounts.Users.Queries;
using Hospital.Domain;
using Moq;
using Xunit;

namespace Hospital.Tests.UsersTests
{
    public class UserAccountQueryTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IUserAccountQuery _iUserAccountQuery;
        private readonly IMapper _mapper;

        public UserAccountQueryTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _iUserAccountQuery = new UserAccountQuery(_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetAll_ShouldBeExpected()
        {
            var users = _fixture.Build<User>().CreateMany();

            await _hospitalContextFixture.InitUsersAsync(users);

            var result = _iUserAccountQuery.GetAll();

            result.Should().NotBeNull();
        }
    }
}