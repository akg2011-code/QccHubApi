using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QccHub.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]/[action]")] // follow this convention for APIs
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Content("Ok");
        }
        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            return Content($"Ok {id}");
        }
    }
}
