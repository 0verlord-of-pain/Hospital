using System;
using System.Collections.Generic;
using System.Linq;
using Hospital.Domain.Enums;

namespace Hospital.Application.CQRS.UserAccounts.Role.Queries
{
    public sealed class UserRoleQuery : IUserRoleQuery
    {
        public List<string> GetRoles()
        {
            var result = Enum.GetValues(typeof(Roles));
            var roles = (from object role in result select role.ToString()).ToList();
            return roles;
        }
    }
}