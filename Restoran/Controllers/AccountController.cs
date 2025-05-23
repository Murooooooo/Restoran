﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restoran.Helper;
using Restoran.Helper.Email;
using Restoran.Models;
using Restoran.Services.Email;
using Restoran.ViewModels;

namespace Restoran.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new()
            {
                Name = registerVM.Name,
                UserName = registerVM.UserName,
                Email = registerVM.Email,

            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, RoleEnum.Member.ToString());

            return RedirectToAction("Login");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (appUser is null)
            {
                appUser = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (appUser is null)
                {
                    ModelState.AddModelError("", "UserNameOrEmail or Password is incorrect");
                    return View();
                }

            }

            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RememberMe, true);


            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(RoleEnum)))
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = item.ToString()
                });
            }
            return Content("Role Created");
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (appUser is null)
            {
                return NotFound();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

            var userStat = new
            {
                userId = appUser.Id,
                token = token
            };
            var link = Url.Action("ResetPassword", "Account", userStat, HttpContext.Request.Scheme);

            MailRequest mailRequest = new()
            {
                ToEmail = forgotPasswordVM.Email,
                Subject = "Reset Password",
                Body = $"<a href='{link}'>Reset Password</a>"
            };
            await _mailService.SendEmailAsync(mailRequest);

            return RedirectToAction("Login");
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            { return BadRequest(); }

            var userStat = new
            {
                userId = userId,
                token = token
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordVM);
            }
            var user = await _userManager.FindByIdAsync(resetPasswordVM.userId);
            if (user is null)
            {
                return NotFound();
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.token, resetPasswordVM.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(Login));

        }


    }
}
