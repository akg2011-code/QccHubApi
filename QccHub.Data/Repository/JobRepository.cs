using Microsoft.EntityFrameworkCore;
using QccHub.Data.Interfaces;
using QccHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data.Repository
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        private ApplicationDbContext _context;
        public JobRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Job>> SearchJobs(string jobName)
        {
            return _context.Job.Where(j => j.Title.Contains(jobName)).ToListAsync();
        }

        public Task<List<Job>> GetJobsByCompany(int companyId)
        {
            return _context.Job.Where(j => j.CompanyID == companyId).Include(j => j.Company).ToListAsync();
        }
    }
}
