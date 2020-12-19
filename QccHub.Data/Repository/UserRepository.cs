﻿using Microsoft.EntityFrameworkCore;
using QccHub.Data.Interfaces;
using QccHub.Data.Models;
using QccHub.Logic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace QccHub.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<ApplicationUser>> GetAllUsers()
        {
            return _context.Users.ToListAsync();
        }

        public Task<List<ApplicationUser>> GetCompanyUsers()
        {
            return _context.Users.IncludeFilter(u => u.UserRoles.Where(ur => ur.RoleId == (int)RolesEnum.Company)).ToListAsync();
        }

        public UserJobPosition GetCurrentJobPosition(int userId)
        {
            return _context.UserJobPositions
                            .Include(ujp => ujp.JobPosition)
                            .IncludeFilter(ujp => ujp.Employee.UserRoles.FirstOrDefault(r => r.RoleId == (int)RolesEnum.User))
                            .FirstOrDefault(ujp => ujp.IsCurrentPosition && ujp.EmployeeId == userId);
        }

        public Task<List<ApplicationUser>> GetEmployeeUsers()
        {
            return _context.Users.IncludeFilter(u => u.UserRoles.Where(ur => ur.RoleId == (int)RolesEnum.User)).ToListAsync();
        }

        public Task<ApplicationUser> GetUserByIdAsync(int userId)
        {
            return _context.Users.Include(u => u.UserRoles)
                                    .Include(x => x.Country)
                                    .Include(x => x.Gender)
                                    .Include(x => x.EmployeeJobs)
                                    .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return _context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<int> GetUserRole(int userId)
        {
            return (await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId)).RoleId;
        }
    }
}
