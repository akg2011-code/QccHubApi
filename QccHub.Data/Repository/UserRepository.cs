using Microsoft.EntityFrameworkCore;
using QccHub.Data.Interfaces;
using QccHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<ApplicationUser> GetUserByIdAsync(int userId)
        {
            return _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}
