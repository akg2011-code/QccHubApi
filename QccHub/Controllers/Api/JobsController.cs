using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QccHub.Data;

namespace QccHub.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        ApplicationDbContext Context;
        public JobsController(ApplicationDbContext _context)
        {
            Context = _context;
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
                IEnumerable<Job> companyJobs = Context.Job.Where(j=>j.CompanyID == companyID).Include(j=>j.Company);
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
            return Ok(Context.Job.OrderByDescending(j=>j.CreatedDate).Include(j=>j.Company));
        }

    }
}