using QccHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QccHub.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserByUserNameAsync(string userName);
        Task<ApplicationUser> GetUserByIdAsync(int userId);
    }
}
