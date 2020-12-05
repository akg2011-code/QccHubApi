using Microsoft.AspNetCore.Http;
using QccHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.DTOS
{
    public class JobApplication
    {
        public string UserID { get; set; }
        public int JobID { get; set; }
        public string Message { get; set; }
        public int CurrentSalary { get; set; }
        public int ExpectedSalary { get; set; }
        public IFormFile cvFile { get; set; }

        public ApplyJobs ToModel(string fileName)
        {
            return new ApplyJobs()
            {
                ExpectedSalary = this.ExpectedSalary,
                CurrentSalary = this.CurrentSalary,
                UserID = this.UserID,
                JobID = this.JobID,
                Message = this.Message,
                CVFilePath = fileName
            };
        }
    }
}