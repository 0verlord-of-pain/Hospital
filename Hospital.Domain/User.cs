using System;
using System.Collections.Generic;
using Hospital.Domain.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Hospital.Domain
{
    public class User : IdentityUser, ICreatedOnUtc, IUpdatedOnUtc
    {
        public List<Record> Records { get; set; }
        public Doctor Doctor { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public static User Create(string surname, string name, string email, string phoneNumber)
        {
            return new User
            {
                UserName = email,
                Surname = surname,
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };
        }

        public User UpdatePhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            return this;
        }

        public User UpdateEmail(string email)
        {
            Email = email;
            return this;
        }
    }
}