using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QccHub.Data.Models;

namespace QccHub.Controllers.Website
{
    public class NewsController : Controller
    {
        HttpClient client = new HttpClient();

        public IActionResult GetAllNews()
        {
            var response = client.GetAsync("https://localhost:44324/api/News/GetAllNews/").Result;
            var body = response.Content.ReadAsStringAsync().Result;

            IEnumerable<News> news = JsonConvert.DeserializeObject<IEnumerable<News>>(body);

            return View(news);
        }

        public IActionResult GetNewsDetails(int id)
        {
            var response = client.GetAsync("https://localhost:44324/api/News/GetNewsByID/" + id).Result;
            var body = response.Content.ReadAsStringAsync().Result;

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
