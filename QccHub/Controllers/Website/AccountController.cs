using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QccHub.DTOS;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;

namespace QccHub.Controllers.Website
{
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        public AccountController(IConfiguration iConfig, IHttpClientFactory clientFactory) : base(iConfig,clientFactory)
        {
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var httpClient = _clientFactory.CreateClient("API");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model),Encoding.UTF8,"application/json");
            var response = await httpClient.PostAsync("Account/Login", jsonContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddModelError(result);
                return View(model);
            }

            var loginResult = JsonConvert.DeserializeObject<LoginResultVM>(result);
            HttpContext.Session.SetString("UserName", loginResult.UserName);
            HttpContext.Session.SetString("AccessToken", loginResult.AccessToken);
            HttpContext.Session.SetString("RoleName", loginResult.RoleName);
            
            var decodedUrl = WebUtility.HtmlDecode(returnUrl);
            if (Url.IsLocalUrl(decodedUrl))
                return Redirect(decodedUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisteration model)
        {
            var httpClient = _clientFactory.CreateClient("API");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Account/Register", jsonContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddModelError(result);
                return View(model);
            }

            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            var httpClient = _clientFactory.CreateClient("API");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("Account/ForgotPassword", jsonContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddModelError(result);
                return View(model);
            }
            return Content("an email sent to you with a link to reset your password");
        }

    }
}
