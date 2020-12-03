using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QccHub.Data;
using QccHub.DTOS;

namespace QccHub.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        ApplicationDbContext Context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public JobsController(ApplicationDbContext _context, IWebHostEnvironment _webHostEnvironment)
        {
            Context = _context;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpPost]
        public IActionResult AddJob(Job job)
        {
            if (ModelState.IsValid)
            {
                job.IsDeleted = false;
                job.CreatedDate = DateTime.Now;
                job.CreatedBy = job.CompanyID;
                Context.Job.Add(job);
                Context.SaveChanges();
                return Ok("job added");
            }
            else
                return BadRequest("can't add , some info is wronge");
        }

        [HttpDelete("{jobID}")]
        public IActionResult DeleteJob(int jobID)
        {
            Job job = Context.Job.Find(jobID);
            if (job == null)
                return NotFound("no job for this ID");
            else
            {
                job.IsDeleted = true;
                Context.SaveChanges();
                return Ok("deleted job");
            }
        }

        [HttpGet("{companyID}")]
        public IActionResult GetAllCompanyJobs(string companyID)
        {
            var company = Context.Users.Find(companyID);
            if (company == null)
                return NotFound("No company for this ID");
            else
            {
                IEnumerable<Job> companyJobs = Context.Job.Where(j => j.CompanyID == companyID).Include(j => j.Company);
                return Ok(companyJobs);
            }
        }

        [HttpGet("{jobID}")]
        public IActionResult GetJob(int jobID)
        {
            Job job = Context.Job.Find(jobID);
            if (job != null)
                return Ok(job);
            else
                return NotFound("No Job for this ID");
        }

        [HttpGet]
        public IActionResult GetAllJobs()
        {
            return Ok(Context.Job.OrderByDescending(j => j.CreatedDate).Include(j => j.Company));
        }

        [HttpGet("{JobName}")]
        public IActionResult SearchJobs(string JobName)
        {
            IEnumerable<Job> jobs = Context.Job.Where(j => j.Title.Contains(JobName) && j.IsDeleted == false).Include(j=>j.Company);
            return Ok(jobs);
        }
        // --------------------------- apply to job ------------------------------
        [HttpPost]
        public IActionResult ApplyToJob([FromForm]JobApplications JobApplication)
        {
            if (JobApplication.cvFile != null && JobApplication.cvFile.Length > 0)
            {
                string cvFileName = Guid.NewGuid().ToString() + JobApplication.cvFile.FileName;
                string cvFilePath = Path.Combine(webHostEnvironment.WebRootPath + "\\JobsAppliedCV", cvFileName);
                using (var stream = new FileStream(cvFilePath, FileMode.Create))
                {
                    JobApplication.cvFile.CopyTo(stream);
                }
                ApplyJobs applyJobs = new ApplyJobs
                {
                    IsApproved = false,
                    ExpectedSalary = JobApplication.ExpectedSalary,
                    CurrentSalary = JobApplication.CurrentSalary,
                    CreatedBy = JobApplication.UserID,
                    CreatedDate = DateTime.Now,
                    UserID = JobApplication.UserID,
                    JobID = JobApplication.JobID,
                    Message = JobApplication.Message,
                    IsDeleted = false,
                    CVFilePath = cvFileName
                };
                
                Context.ApplyJobs.Add(applyJobs);
                Context.SaveChanges();
                return Ok($"user applyed to job");
            }
            else
                return BadRequest();
        }

        [HttpGet("{jobID}")]
        public IActionResult GetAllJobApplications(int jobID)
        {
            Job job = Context.Job.Find(jobID);
            if (job == null)
                return NotFound("No job found for this ID");
            IEnumerable<ApplyJobs> jobApplications = Context.ApplyJobs.Where(j=>j.JobID == jobID).Include(j=>j.User).Include(j=>j.Job);
            return Ok(jobApplications);
        }

        [HttpPost()]
        public IActionResult ApproveJobApplication(JobApprove jobApprove)
        {
            ApplyJobs applyJobs = Context.ApplyJobs.FirstOrDefault(j=>j.JobID == jobApprove.JobID && j.UserID == jobApprove.UserID);
            if (applyJobs != null)
            {
                applyJobs.IsApproved = true;
                Context.SaveChanges();
                return Ok($"user : {Context.Users.Find(applyJobs.UserID).UserName} is approved to job : {Context.Job.Find(applyJobs.JobID).Title}");
            }
            else
                return NotFound();
        }

    }
}