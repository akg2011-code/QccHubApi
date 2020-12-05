using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QccHub.Data.Interfaces
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<List<Job>> SearchJobs(string jobName);
        Task<List<Job>> GetJobsByCompany(string CompanyId);
    }
}
