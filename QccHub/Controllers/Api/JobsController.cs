using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


    }
}