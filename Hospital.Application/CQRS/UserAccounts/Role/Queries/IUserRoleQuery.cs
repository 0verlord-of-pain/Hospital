using System.Collections.Generic;
namespace Hospital.Application.CQRS.UserAccounts.Role.Queries
{
    public interface IUserRoleQuery
    {
        public List<string> GetRoles();
    }
}