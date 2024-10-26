using LigaNOS.Data.Entities;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LigaNOS.Data.Repositories;


namespace LigaNOS.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IConfiguration _configuration;
       
        private readonly IUserRepository _userRepository;
        private readonly IBlobHelper _blobHelper;

        public AccountController(
            IUserHelper userHelper,
            IMailHelper mailHelper,
            IConfiguration configuration,

            IUserRepository userRepository,
            IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _configuration = configuration;

            _userRepository = userRepository;
            _blobHelper = blobHelper;
        }
            // GET: AccountController

            public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userHelper.LoginAsync(model);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            this.ModelState.AddModelError(string.Empty, "Failed to login");
            return View(model);
        }
        


            [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }


        [Authorize(Roles = "Admin, Club, Emplo ")]
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            { 
                   var  user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //token
                    var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    //link
                    var confirmationLink = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, token },
                        Request.Scheme);

                    // Email
                    //_mailHelper.SendEmail(user.Email, "E-mail confirmation", $"Click on link below to confirm your e-mail. {confirmationLink}");

                    
                    ViewBag.Message = "Please, confirm your email.";
                    return View("Register");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error!");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.Message = "Email confirmation succeeded!";
                return View("ConfirmEmail");
            }

            ViewBag.Message = "Email confirmation not succeeded. Contact our platform support";
            return View("Error");
        }

        // [Authorize(Roles = "Admin, Club, Emplo")]
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User updated!";

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        //[Authorize(Roles = "Admin, Club, Emplo ")]
        public IActionResult ChangePassword()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found");

                }
            }
            return this.View(model);
        }

        public IActionResult NotAuthorized()
        {
            return View("NotAuthorized");
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Info missing.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Password Reset", $"<h1>Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }


        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.Username);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }
        public IActionResult TestEmail()
        {
            var response = _mailHelper.SendEmail("miguens.rpm@gmail.com", "Test Subject", "<h1>This is a test</h1>");
            if (response.IsSuccess)
            {
                return Content("Email sent successfully.");
            }
            else
            {
                return Content($"Error sending email: {response.Message}");
            }
        }
    }
}