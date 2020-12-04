using QccHub.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QccHub.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<User> GetUserById(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
