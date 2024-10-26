using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using LigaNOS.Models;
using System;

namespace LigaNOS.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole>roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            try
            {
                return await _userManager.CreateAsync(user, password);
            }
            catch (Exception ex)
            {
                 
                throw new InvalidOperationException($"Error creating user: {ex.Message}", ex);
            }

        }
        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
           

            var result = await _signInManager.PasswordSignInAsync(
       model.Username,
       model.Password,
       model.RememberMe,
       lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return result;
            }
            else if (result.IsLockedOut)
            {
                throw new InvalidOperationException("User blocked.");
            }
            else if (result.IsNotAllowed)
            {
                throw new InvalidOperationException("You need to validate your e-mail.");
            }
            else
            {
                throw new InvalidOperationException("Check details");
            }
            return result;
        }
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });

            }
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }
    }
}
