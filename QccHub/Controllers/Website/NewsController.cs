using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QccHub.Data.Models;
using Microsoft.Extensions.Configuration;

namespace QccHub.Controllers.Website
{
    public class NewsController : BaseController
    {
        public NewsController(IConfiguration iConfig, IHttpClientFactory clientFactory) : base(iConfig, clientFactory)
        {
        }

        public async Task<IActionResult> GetAllNews()
        {
            var httpClient = _clientFactory.CreateClient("API");

            var response = await httpClient.GetAsync("News/GetAllNews/");
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("index","home");
            }

            IEnumerable<News> news = JsonConvert.DeserializeObject<IEnumerable<News>>(body);
            return View(news);
        }

        public async Task<IActionResult> GetNewsDetails(int id)
        {
            var httpClient = _clientFactory.CreateClient("API");
            var response = await httpClient.GetAsync("News/GetNewsByID/" + id);
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("index", "home");
            }

            News news = JsonConvert.DeserializeObject<News>(body);
            return View(news);
        }

        public IActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNews(News news)
        {
            if (!ModelState.IsValid)
            {
                return View(news);
            }

            return RedirectToAction("GetAllNews");
        }
    }
}
