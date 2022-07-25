using System.Threading.Tasks;
using Hospital.Domain;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.CQRS.UserAccounts.Validator
{
    public class PasswordValidator
    {
        private readonly UserManager<User> _userManager;

        public PasswordValidator(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsValidAsync(string password)
        {
            return (await new PasswordValidator<User>()
                .ValidateAsync(_userManager, null, password)).Succeeded;
        }
    }
}