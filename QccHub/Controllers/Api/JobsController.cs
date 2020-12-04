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
using QccHub.Data.Interfaces;
using QccHub.DTOS;

namespace QccHub.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepository _jobRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public JobsController(IJobRepository jobRepo, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _jobRepo = jobRepo;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddJob(Job job)    // note: should not recieve or return domain model , use DTO instead
        {
            if (!ModelState.IsValid)                        // make a condition for the non-happy scenarios first to avoid code branching, this is called a guard
            {
                return BadRequest("can't add , some info is wrong");
            }
            _jobRepo.Add(job);                              // you should use repository pattern to decouple the database connection and logic from api
            await _unitOfWork.SaveChangesAsync();           // you should use async when dealing with file system, databases or third-party services
            return Created("job added", job);               // return a DTO of the created object
        }

        [HttpDelete("{jobID}")]
        public async Task<IActionResult> DeleteJob(int jobID)
        {
            Job job = await _jobRepo.GetByIdAsync(jobID);
            if (job == null)
            {
                return NotFound("no job for this ID");
            }
            // no need for an else block
            _jobRepo.Delete(job); // delete will set IsDeleted to True when SaveChangesAsync is Called using shadow properties
            await _unitOfWork.SaveChangesAsync();
            return Ok("deleted job");
            
        }

        //[HttpGet("{companyID}")]
        //public IActionResult GetAllCompanyJobs(string companyID)
        //{
        //    var company = Context.Users.Find(companyID);
        //    if (company == null)
        //        return NotFound("No company for this ID");
        //    else
        //    {
        //        IEnumerable<Job> companyJobs = Context.Job.Where(j => j.CompanyID == companyID).Include(j => j.Company);
        //        return Ok(companyJobs);
        //    }
        //}

        [HttpGet("{jobID}")]
        public async Task<IActionResult> GetJob(int jobID)
        {
            Job job = await _jobRepo.GetByIdAsync(jobID);
            if (job == null)
            {
                return NotFound("No Job for this ID");
            }
            return Ok(job);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllJobs()
        {
            var result = await _jobRepo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{JobName}")]
        public async Task<IActionResult> SearchJobs(string JobName)
        {
            var jobs = await _jobRepo.SearchJobs(JobName);
            return Ok(jobs);
        }
        // --------------------------- apply to job ------------------------------
        [HttpPost]
        public async Task<IActionResult> ApplyToJob([FromForm]JobApplication JobApplication)
        {
            if (JobApplication.cvFile != null && JobApplication.cvFile.Length > 0)
            {
                string cvFileName = Guid.NewGuid().ToString() + JobApplication.cvFile.FileName;
                string cvFilePath = Path.Combine(_webHostEnvironment.WebRootPath + "\\JobsAppliedCV", cvFileName);
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
                
                //Context.ApplyJobs.Add(applyJobs);   add a method in the IJobRepository interface and implement it
                await _unitOfWork.SaveChangesAsync();
                return Ok($"user applyed to job");
            }
            else
                return BadRequest();
        }

        [HttpGet("{jobID}")]
        public async Task<IActionResult> GetAllJobApplications(int jobID)
        {
            var jobApplications = await _jobRepo.GetJobApplicationsByJob(jobID);
            return Ok(jobApplications);
        }

        [HttpPost()]
        public async Task<IActionResult> ApproveJobApplication(JobApprove jobApprove)
        {
            var jobApplication = await _jobRepo.GetJobApplicationsByUserAndJob(jobApprove.UserID, jobApprove.JobID);
            if (jobApplication == null)
            {
                return NotFound();
            }

            jobApplication.IsApproved = true;
            await _unitOfWork.SaveChangesAsync();
            return Ok("approved"); // will change this after user repository creation
        }

    }
}