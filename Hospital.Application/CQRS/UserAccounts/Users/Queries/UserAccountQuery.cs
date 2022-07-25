using System.Collections.Generic;
using AutoMapper;
using Hospital.Application.CQRS.UserAccounts.Views;
using Hospital.Storage.Persistence;

namespace Hospital.Application.CQRS.UserAccounts.Users.Queries
{
    public sealed class UserAccountQuery : IUserAccountQuery
    {
        private static HospitalContext _context;
        private static IMapper _mapper;

        public UserAccountQuery(
            HospitalContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<UserAccountView> GetAll()
        {
            var result = _context.Users;
            return _mapper.Map<IEnumerable<UserAccountView>>(result);
        }
    }
}