using Microsoft.EntityFrameworkCore;
using QccHub.Data.Interfaces;
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

        public Task<List<ApplyJobs>> GetJobApplicationsByJob(int jobID)
        {
            return _context.ApplyJobs.Where(j => j.JobID == jobID).Include(j => j.User).Include(j => j.Job).ToListAsync();
        }

        public Task<ApplyJobs> GetJobApplicationsByUserAndJob(string userId, int jobId)
        {
            return _context.ApplyJobs.FirstOrDefaultAsync(j => j.JobID == jobId && j.UserID == userId);
        }
    }
}
