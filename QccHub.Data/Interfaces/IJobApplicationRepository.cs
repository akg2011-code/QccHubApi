using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Data.Interfaces
{
    public interface IJobApplicationRepository : IGenericRepository<ApplyJobs>
    {
        Task<List<ApplyJobs>> GetJobApplicationsByJob(int jobID);
        Task<ApplyJobs> GetJobApplicationsByUserAndJob(string userId, int jobId);
    }
}
