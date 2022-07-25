using System.Collections.Generic;
using Hospital.Application.CQRS.UserAccounts.Views;

namespace Hospital.Application.CQRS.UserAccounts.Users.Queries
{
    public interface IUserAccountQuery
    {
        public IEnumerable<UserAccountView> GetAll();
    }
}