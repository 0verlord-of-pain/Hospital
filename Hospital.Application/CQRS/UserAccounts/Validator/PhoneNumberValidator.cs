using System.Text.RegularExpressions;

namespace Hospital.Application.CQRS.UserAccounts.Validator
{
    public class PhoneNumberValidator
    {
        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;
            var regex = new Regex(@"^\+?[1-9][0-9]{7,12}$");
            return regex.IsMatch(phoneNumber);
        }
    }
}