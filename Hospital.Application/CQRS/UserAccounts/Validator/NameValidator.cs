using System.Text.RegularExpressions;

namespace Hospital.Application.CQRS.UserAccounts.Validator
{
    public class NameValidator
    {
        public static bool IsValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var regex = new Regex(@"^([a-zA-Z]+[ '-]?)+$");
            return regex.IsMatch(name);
        }
    }
}