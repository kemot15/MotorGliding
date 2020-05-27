using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class AccountController : Controller
    {
        protected UserManager<User> UserManager { get; }
        protected SignInManager<User> SignInManager { get; }
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IAccountService accountService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            ViewBag.Error = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromForm]RegLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.RegistrationViewModel.UserName,
                    Email = model.RegistrationViewModel.Email
                };
                var result = await UserManager.CreateAsync(user, model.RegistrationViewModel.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewBag.Error = true;
            return View(model);
        }

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Login(RegLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                //if (result.IsLockedOut)
                //{
                //    ModelState.AddModelError("", "Uzytkownik jest zablokowany");
                //   // return View(loginModel);
                //}

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Uzytkownik jest zablokowany");
                }
                else
                {
                    ModelState.AddModelError("", "Błąd logowania");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            ViewBag.Active = "Account";
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
                return View();//redirect to Login?
            var address = await _accountService.GetUserAddress(user.Id);
            if (address != null) user.Address = address;
            else
                user.Address = new Address();
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Street = user.Address.Street,
                ZipCode = user.Address.ZipCode,
                City = user.Address.City,
                Country = user.Address.Country
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            ViewBag.Active = "Account";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.GetUserAsync(User);
            if (user == null) return View("Index", "Dashboard");
                
            user.Address = await _accountService.GetUserAddress(user.Id);
            if (user.Address == null)
                user.Address = new Address();
            user.Name = model.Name;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;            
            user.Address.Street = model.Street;
            user.Address.ZipCode = model.ZipCode;
            user.Address.City = model.City;
            user.Address.Country = model.Country;

            await UserManager.UpdateAsync(user);
            //return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditPass(EditPassViewModel model)
        {
            if (!ModelState.IsValid) return View();

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            //var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            await UserManager.ChangePasswordAsync(user, model.OldPass, model.Password);  //ResetPasswordAsync(user, token, model.Password);
            ViewBag.Status = "Hasło zostało pomyślnie zmienione";
            //return Validation("Index", "Dashboard", result, null);
            return View();
        }

        public IActionResult Validation(string action, string controller, IdentityResult result, Object model)
        {
            if (result.Succeeded)
            {
                return RedirectToAction(action, controller);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

    }
}
