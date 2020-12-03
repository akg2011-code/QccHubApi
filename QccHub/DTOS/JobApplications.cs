using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.DTOS
{
    public class JobApplications
    {
        public string UserID { get; set; }
        public int JobID { get; set; }
        public string Message { get; set; }
        public int CurrentSalary { get; set; }
        public int ExpectedSalary { get; set; }
        public IFormFile cvFile { get; set; }
    }
}
