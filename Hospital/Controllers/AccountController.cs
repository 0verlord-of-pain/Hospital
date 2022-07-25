using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hospital.Application.CQRS.UserAccounts.Users.Queries;
using Hospital.Application.CQRS.UserAccounts.Validator;
using Hospital.Application.CQRS.UserAccounts.Views;
using Hospital.Application.Extensions;
using Hospital.Domain;
using Hospital.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/users")]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountQuery _iUserAccountQuery;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserAccountQuery iUserAccountQuery)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _iUserAccountQuery = iUserAccountQuery;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(
            [FromQuery] string email,
            [FromQuery] string surname,
            [FromQuery] string name,
            [FromQuery] string phoneNumber,
            [FromQuery] string password)
        {
            if (!EmailValidator.IsValid(email)) throw new ArgumentException("Email is not valid");

            if (!NameValidator.IsValid(surname)) throw new ArgumentException("Surname is not valid");

            if (!NameValidator.IsValid(name)) throw new ArgumentException("Name is not valid");

            if (!PhoneNumberValidator.IsValid(phoneNumber)) throw new ArgumentException("Phone number is not valid");

            if (!await new PasswordValidator(_userManager).IsValidAsync(password))
                throw new ArgumentException("Password is not valid");

            var user = Domain.User.Create(surname, name, email, phoneNumber);

            var result = await _userManager.CreateAsync(user, password);

            if (result.TryGetErrors(out _)) return BadRequest();

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

            if (addToRoleResult.TryGetErrors(out _)) return BadRequest();

            return Ok();
        }

        [HttpPut("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(
            [FromQuery] string email,
            [FromQuery] string password)
        {
            var userName = email;

            if (EmailValidator.IsValid(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null) userName = user.UserName;
            }

            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (!result.Succeeded) return BadRequest();

            return Ok();
        }

        [HttpPut("logout")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet]
        [Authorize(Policy = Policies.AdminOrManager)]
        public IEnumerable<UserAccountView> GetUsers()
        {
            return _iUserAccountQuery.GetAll();
        }
    }
}