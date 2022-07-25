using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Application.Extensions
{
    public static class IdentityResultExtensions
    {
        public static bool TryGetErrors(this IdentityResult result, out string errors)
        {
            if (result.Succeeded)
            {
                errors = string.Empty;
                return false;
            }

            errors = result.Errors
                .Select(i => i.Description)
                .Aggregate((x, y) => $"{x} \n {y}");

            return true;
        }
    }
}