using QccHubApi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QccHubApi.Models
{
    public class ApplyJobs : Entities
    {
        [ForeignKey("User")]
        public string UserID { get; set; }
        [ForeignKey("Job")]
        public int JobID { get; set; }
        public string Message { get; set; }
        public int CurrentSalary { get; set; }
        public int ExpectedSalary { get; set; }
        public string CVFilePath { get; set; }
        public bool IsApproved { get; set; }
        public User User { get; set; }
        public Job Job { get; set; }
    }
}
