﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QccHub.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameAsync(string userName);
        Task<User> GetUserByIdAsync(string userId);
    }
}
