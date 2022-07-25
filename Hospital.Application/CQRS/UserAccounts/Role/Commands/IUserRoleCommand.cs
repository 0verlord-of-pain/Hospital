using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.UserAccounts.Role.Views;

namespace Hospital.Application.CQRS.UserAccounts.Role.Commands
{
    public interface IUserRoleCommand
    {
        public Task<UserRoleView> AddUserRoleAsync(Guid userId, string role);
        public Task<UserRoleView> RemoveUserRoleAsync(Guid userId, string role);
    }
}