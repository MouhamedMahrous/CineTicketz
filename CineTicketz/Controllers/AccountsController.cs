using CineTicketz.Data.Static;
using CineTicketz.Models;
using CineTicketz.Services;
using CineTicketz.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CineTicketz.Controllers
{
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _unitOfWork.Users.GetAllUsers();

            return View(users);
        }

        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();

            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.EmailAddress);

            if (user is not null)
            {
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

                if (isPasswordValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Movies");
                    }
                }
            }

            TempData["Error"] = "Wrong credentials, try again!";
            return View(model);
        }

        public IActionResult Register()
        {
            var viewModel = new RegisterViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.EmailAddress);

            if (user is not null)
            {
                TempData["Error"] = "This Email address already exists!";
                return View(model);
            }

            user = new ApplicationUser()
            {
                FullName = model.FullName,
                UserName = model.EmailAddress,
                Email = model.EmailAddress
            };

            var isUserCreated = await _userManager.CreateAsync(user, model.Password);

            if (isUserCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppUserRoles.User);
                return View("RegisterCompleted");
            }

            TempData["Error"] = "Your password must be at least 8 chars long containing chars, numbers, and symbols";

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Movies");
        }
    }
}
