using System;
using System.Collections.Generic;

namespace Hospital.Application.CQRS.UserAccounts.Role.Views
{
    public class UserRoleView
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}