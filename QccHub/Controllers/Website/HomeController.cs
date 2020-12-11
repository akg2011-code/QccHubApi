using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace QccHub.Controllers.Website
{
    [Route("[controller]/[action]/{id?}")]
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration iConfig, IHttpClientFactory clientFactory) : base(iConfig, clientFactory)
        {
        }
        public IActionResult Index() 
        {
            return View();
        }
    }
}
