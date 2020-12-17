using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QccHub.DTOS;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using QccHub.Data.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Linq;
using QccHub.Logic.Enums;

namespace QccHub.Controllers.Website
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        public AccountController(IConfiguration iConfig, IHttpClientFactory clientFactory) : base(iConfig, clientFactory)
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
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
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
                return RedirectToAction(nameof(Profile), new { id = loginResult.UserId });
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

            return RedirectToAction("Profile", "Account");
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Profile(int id)
        {
            var httpClient = _clientFactory.CreateClient("API");
            var response = await httpClient.GetAsync($"Account/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddModelError(result);
                return RedirectToAction("Index", "Home");
            }
            var user = JsonConvert.DeserializeObject<ApplicationUser>(result);
            if (user.UserRoles.FirstOrDefault().RoleId == (int)RolesEnum.Company)
            {
                return View("CompanyProfile", user);
            }
            else if (user.UserRoles.FirstOrDefault().RoleId == (int)RolesEnum.User)
            {
                return View(user);
            }

            return Content("Error");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> UpdateInfo(int id)
        {
            var httpClient = _clientFactory.CreateClient("API");
            var response = await httpClient.GetAsync($"Account/{id}?update=true");
            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddModelError(result);
                return RedirectToAction("Index", "Home");
            }
            var userUpdateVM = JsonConvert.DeserializeObject<UpdateInfoVM>(result);

            return View(userUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo(int Id, UpdateInfoVM model)
        {
            var httpClient = _clientFactory.CreateClient("API");
            byte[] CVBytes;
            byte[] PPBytes;
            var multipartContent = new MultipartFormDataContent();

            if (model.CV != null && model.CV.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.CV.CopyTo(ms);
                    CVBytes = ms.ToArray();
                    var byteArrayContent = new ByteArrayContent(CVBytes);
                    multipartContent.Add(byteArrayContent, "CV", model.CV.FileName);
                }

            }
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    model.ProfileImage.CopyTo(ms);
                    PPBytes = ms.ToArray();
                    var byteArrayContent = new ByteArrayContent(PPBytes);
                    multipartContent.Add(byteArrayContent, "ProfileImage", model.ProfileImage.FileName);
                }
            }


            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            var response = await httpClient.PostAsync($"Account/UpdateInfo", multipartContent);

            var result = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                AddModelError(result);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Profile", "Account");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> ChangeProfilePicture(int id, IFormFile file)
        {
            var httpClient = _clientFactory.CreateClient("API");
            byte[] PPBytes;
            var multipartContent = new MultipartFormDataContent();

            if (file == null || file.Length == 0)
            {
                return BadRequest("file seems to be empty");
            }
            
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                PPBytes = ms.ToArray();
                var byteArrayContent = new ByteArrayContent(PPBytes);
                multipartContent.Add(byteArrayContent, "file", file.FileName);
            }

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            var response = await httpClient.PostAsync($"Account/ChangeProfilePicture/{id}", multipartContent);
            var result = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(result);
            }
            
            return Ok(result);
        }

    }
}
