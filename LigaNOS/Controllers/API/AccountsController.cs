using LigaNOS.Data.Entities;
using LigaNOS.Helpers;
using LigaNOS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace LigaNOS.Controllers.API
{
    public class AccountsController
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AccountController : ControllerBase
        {
            private readonly IUserHelper _userHelper;
            private readonly IMailHelper _mailHelper;
            private readonly IConfiguration _configuration;

            public AccountController(
                IUserHelper userHelper,
                IMailHelper mailHelper,
                IConfiguration configuration)
            {
                _userHelper = userHelper;
                _mailHelper = mailHelper;
                _configuration = configuration;
            }

            [HttpPost("CreateToken")]
            public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null || !(await _userHelper.ValidatePasswordAsync(user, model.Password)).Succeeded)
                {
                    return Unauthorized("Invalid username or password.");
                }

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

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            [HttpPost("Register")]
            public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Username,
                    UserName = model.Username,
                };

                var result = await _userHelper.AddUserAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
                }

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Account",
                    new { userId = user.Id, token },
                    Request.Scheme);

                var response = _mailHelper.SendEmail(
                    user.Email,
                    "Confirm Your Email",
                    $"Click here to confirm your email: {confirmationLink}");

                if (!response.IsSuccess)
                {
                    return BadRequest("Error sending confirmation email.");
                }

                return Ok("User registered successfully. Please confirm your email.");
            }

            [HttpPost("RecoverPassword")]
            public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordViewModel model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null) return BadRequest("User not found.");

                var token = await _userHelper.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token }, Request.Scheme);

                var response = _mailHelper.SendEmail(
                    model.Email,
                    "Reset Your Password",
                    $"Click here to reset your password: {resetLink}");

                if (!response.IsSuccess)
                {
                    return BadRequest("Error sending reset email.");
                }

                return Ok("Instructions to reset your password have been sent.");
            }

            [HttpPost("ChangePassword")]
            [Authorize]
            public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user == null) return NotFound("User not found.");

                var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.Select(e => e.Description));
                }

                return Ok("Password changed successfully.");
            }
        }
    }
}
