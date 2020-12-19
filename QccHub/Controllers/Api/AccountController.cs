using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using QccHub.Data.Models;
using QccHub.Data.Interfaces;
using QccHub.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using QccHub.Logic.Enums;
using System.IdentityModel.Tokens.Jwt;
using QccHub.Helpers;
using QccHub.Data.Extensions;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using QccHub.Logic.Helpers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace QccHub.Controllers.Api
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepo;
        private readonly IJobPositionRepository _jobPosRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IUserRepository userRepo,
                                  IJobPositionRepository jobPosRepository,
                                  IUnitOfWork unitOfWork,
                                  IEmailSender emailSender,
                                  IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepo = userRepo;
            _jobPosRepo = jobPosRepository;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("api/Account/Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Error Data");
            }

            var user = await _userRepo.GetUserByUserNameAsync(model.Email);
            if (user == null)
                return BadRequest("Incorrect ID or Password");

            try
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return BadRequest("Incorrect ID or Password");
                }

                var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserName),
                            new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                            new Claim(ClaimTypes.Role,  userRole),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                string userToken = GenerateToken.GenerateJSONWebToken(claims);

                HttpContext.Session.SetString("JWToken", userToken);

                LoginResultVM result = new LoginResultVM()
                {
                    AccessToken = userToken,
                    UserName = user.UserName,
                    RoleName = userRole,
                    UserId = user.Id
                };

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/Account/Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost]
        [Route("api/Account/Register")]
        [ProducesResponseType(typeof(UserRegisteration),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserRegisteration),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisteration model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                CompanyName = model.CompanyName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string error = result.GetErrors();
                return BadRequest(error);
            }

            await _userManager.AddToRoleAsync(user, ((RolesEnum)model.RoleId).ToString());
            await _unitOfWork.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: true);
            return Ok(user);
        }

        [HttpGet]
        [Route("api/Account/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("api/Account/GetUserUpdateViewModel/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(UpdateInfoVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserUpdateViewModel(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            var model = new UpdateInfoVM
            {
                Id = user.Id,
                Bio = user.Bio,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                GenderID = user.GenderID,
                NationalityID = user.NationalityID ?? 0,
                ProfileImagePath = user.ProfileImagePath
            };

            if (user.UserRoles.FirstOrDefault()?.RoleId == (int)RolesEnum.User)
            {
                model.Position = _userRepo.GetCurrentJobPosition(id)?.JobPosition?.Name;
            }
            return Ok(model);
        }

        [HttpGet]
        [Route("api/Account/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                return BadRequest();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("api/Account/ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordVM model)
        {
            var canGetUserId = int.TryParse(User.GetUserId(),out int userId);
            if (!canGetUserId)
            {
                return BadRequest("User doesn't exist");
            }

            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/Account/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{model.Email}'.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = HttpUtility.UrlEncode(token);

            string websiteUrl = ConfigValueProvider.Get("AppSettings:WebsiteUrl");
            var callback = $"{websiteUrl}Account/ResetPassword?token={token}&email={user.Email}";
            string mailBody = $"<h4>Somebody recently asked to reset your password.<a href='{callback}'> Click here to change your password.</a></h4>";
            await _emailSender.SendEmailAsync(user.Email, "Reset password", mailBody);

            return Ok();
        }

        [HttpPost]
        [Route("api/Account/ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound($"Unable to load user with email '{model.Email}'.");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPassResult.Succeeded)
            {
                string error = resetPassResult.GetErrors();
                return BadRequest(error);
            }

            return Ok();
        }

        [HttpPost]
        [Route("api/Account/ChangeProfilePicture/{id}")]
        public async Task<IActionResult> ChangeProfilePicture([FromRoute] int id,[FromForm] IFormFile file)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var validExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!validExtensions.Contains(Path.GetExtension(file.FileName)))
            {
                return BadRequest("File extension is not supported");
            }

            var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Profile Pictures");
            var result = await FileUploader.Upload(directoryPath, file);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Uploading failed");
            }

            user.ProfileImagePath = result;
            if (!(await _unitOfWork.SaveChangesAsync() > 0))
            {
                return BadRequest("Uploading failed");
            }

            return Ok(user.ProfileImagePath);
        }

        [HttpDelete]
        [Route("api/Account/DeleteProfilePicture/{id}")]
        public async Task<IActionResult> DeleteProfilePicture([FromRoute] int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "Profile Pictures" , user.ProfileImagePath);
            FileUploader.Delete(directoryPath);
            user.ProfileImagePath = null;
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch]
        [Route("api/Account/UpdateBasicInfo")]
        public async Task<IActionResult> UpdateBasicInfo([FromBody] BasicInfoVM model)
        {
            var user = await _userRepo.GetUserByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.DateOfBirth = model.DateOfBirth;
            user.PhoneNumber = model.PhoneNumber;
            user.GenderID = model.GenderID;
            user.NationalityID = model.NationalityID;

            var currentJobPosition = _userRepo.GetCurrentJobPosition(model.Id);
            user.ResetJobPositions();
            var duplicatedJob = await _jobPosRepo.GetJobPositionByName(model.Position);
            if (duplicatedJob == null)
            {
                user.AddNewJobByName(model.Position, currentJobPosition.CompanyId);
            }
            else
            {
                user.EmployeeJobs.Add(new UserJobPosition 
                { 
                    CompanyId = currentJobPosition.CompanyId,
                    JobPositionId = duplicatedJob.ID,
                    FromDate = DateTime.UtcNow,
                    IsCurrentPosition = true
                }
                );
            }
            //currentJobPosition.ToDate = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch]
        [Route("api/Account/UpdateBio")]
        public async Task<IActionResult> UpdateBio(UpdateBioVM model)
        {
            var user = await _userRepo.GetUserByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Bio = model.Bio;
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

    }
}
