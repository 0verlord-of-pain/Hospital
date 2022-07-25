using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hospital.Application.CQRS.UserAccounts.Role.Queries;
using Hospital.Domain.Enums;
using Xunit;

namespace Hospital.Tests.RoleTests
{

    public class UserRoleQueryTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IUserRoleQuery _iUserRoleQuery;

        public UserRoleQueryTests()
        {
            _iUserRoleQuery = new UserRoleQuery();
        }

        [Fact]
        public void GetAll_ShouldBeExpected()
        {

            var result = _iUserRoleQuery.GetRoles();

            result.Should().NotBeNull();

            foreach (var role in result)
            {
                Enum.IsDefined(typeof(Roles), role).Should().BeTrue();
            }
        }
    }
}