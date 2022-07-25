using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.UserAccounts.Role.Commands;
using Hospital.Application.CQRS.UserAccounts.Role.Queries;
using Hospital.Domain.Enums;
using Hospital.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/users")]
    public class UserRoleController : Controller
    {
        private static IUserRoleCommand _iUserRoleCommand;
        private static IUserRoleQuery _iUserRoleQuery;

        public UserRoleController(
            IUserRoleCommand iUserRoleCommand,
            IUserRoleQuery iUserRoleQuery)
        {
            _iUserRoleCommand = iUserRoleCommand;
            _iUserRoleQuery = iUserRoleQuery;
        }

        [HttpPut("{userId}/role/{role}/attach")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> AttachRole(
            [FromRoute] Guid userId,
            [FromRoute] Roles role)
        {
            var result = await _iUserRoleCommand.AddUserRoleAsync(userId, role.ToString());
            return Ok(result);
        }

        [HttpPut("{userId}/role/{role}/remove")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> RemoveRole(
            [FromRoute] Guid userId,
            [FromRoute] Roles role)
        {
            var result = await _iUserRoleCommand.RemoveUserRoleAsync(userId, role.ToString());
            return Ok(result);
        }

        [HttpGet("roles")]
        [Authorize(Policy = Policies.Admin)]
        public IActionResult Get()
        {
            var result = _iUserRoleQuery.GetRoles();
            return Ok(result);
        }
    }
}