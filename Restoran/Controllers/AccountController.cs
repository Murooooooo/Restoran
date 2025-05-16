using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restoran.Helper;
using Restoran.Models;
using Restoran.ViewModels;

namespace Restoran.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
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

            await _userManager.AddToRoleAsync(user,RoleEnum.Member.ToString());

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

    }
}
