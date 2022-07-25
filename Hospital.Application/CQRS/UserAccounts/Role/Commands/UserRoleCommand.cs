using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hospital.Application.CQRS.UserAccounts.Role.Views;
using Hospital.Application.Extensions;
using Hospital.Domain;
using Hospital.Storage.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.CQRS.UserAccounts.Role.Commands
{
    public sealed class UserRoleCommand : IUserRoleCommand
    {
        private static UserManager<User> _userManager;
        private static HospitalContext _context;
        private static IMapper _mapper;

        public UserRoleCommand(
            UserManager<User> userManager,
            HospitalContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserRoleView> AddUserRoleAsync(Guid userId, string role)
        {
            var user = await _context.Users.FindAsync(userId.ToString());
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.TryGetErrors(out var error))
            {
                throw new ArgumentException(error);
            }

            var userView = _mapper.Map<UserRoleView>(user);

            var rolesIds = _context.UserRoles
                .Where(i => i.UserId.Equals(user.Id))
                .Select(i => i.RoleId);

            userView.Roles = await _context.Roles
                .Where(i => rolesIds.Contains(i.Id))
                .Select(i => i.Name)
                .ToListAsync();

            return userView;
        }

        public async Task<UserRoleView> RemoveUserRoleAsync(Guid userId, string role)
        {
            var user = await _context.Users.FindAsync(userId.ToString());
            var result = await _userManager.RemoveFromRoleAsync(user, role);

            if (result.TryGetErrors(out var error))
            {
                throw new ArgumentException(error);
            }

            var userView = _mapper.Map<UserRoleView>(user);

            var rolesIds = _context.UserRoles
                .Where(i => i.UserId.Equals(user.Id))
                .Select(i => i.RoleId);

            userView.Roles = await _context.Roles
                .Where(i => rolesIds.Contains(i.Id))
                .Select(i => i.Name)
                .ToListAsync();

            return userView;
        }
    }
}