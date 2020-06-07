using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;
using MotorGliding.Models.Db;
using MotorGliding.Models.Enums;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class AccountController : Controller
    {
        protected UserManager<User> UserManager { get; }
        protected SignInManager<User> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager { get; }
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IAccountService accountService, RoleManager<IdentityRole<int>> roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _accountService = accountService;
            RoleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            ViewBag.Error = false;
            ViewBag.Tab = Tabs.Other;
            //ViewData["Title"] = Tabs.Other;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromForm]RegistrationViewModel model)
        {
            //ViewData["Title"] = Tabs.Other;
            ViewBag.Tab = Tabs.Other;
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewBag.Error = true;
            
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Error = false;
            ViewBag.Tab = Tabs.Other;
            //ViewData["Title"] = Tabs.Other;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //ViewData["Title"] = Tabs.Other;
            ViewBag.Tab = Tabs.Other;
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.Login, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Uzytkownik jest zablokowany");
                }
                else
                {
                    ModelState.AddModelError("", "Błąd logowania");
                }
            }
            ViewBag.Error = true;
           // model.ActiveTab = Tab.Login;            
            return View(model);
            //return Json(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id = 0)
        {
            ViewBag.Active = Tabs.UserSettings;

            var user = id == 0 ? await UserManager.GetUserAsync(User) : await _accountService.GetUser(id);
            if (user == null)
                return RedirectToAction("Login","Account");//redirect to Login?
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
        [Authorize]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            ViewBag.Active = Tabs.UserSettings;
            //ViewBag.Active = "Account";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.GetUserAsync(User);
            if (user == null) return View("Login", "Account");
                
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

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.Status = "Dane zostały pomyślnie zmienione";
                return View(model);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            //return RedirectToAction("Index", "Dashboard");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditPass()
        {
            ViewBag.Active = Tabs.UserSettings;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPass(EditPassViewModel model)
        {
            ViewBag.Active = Tabs.UserSettings;
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            //var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var result = await UserManager.ChangePasswordAsync(user, model.OldPass, model.Password);  //ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                ViewBag.Status = "Hasło zostało pomyślnie zmienione";
                return View();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            
            return View(model);
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList(string selectedRole = "0")
        {
            var roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem {Text = "Wszystkie role", Value = "0" });
            foreach (var role in RoleManager.Roles)
            {
                roleList.Add(new SelectListItem { Text = role.Name, Value = role.Id.ToString() });
            }
            ViewBag.RoleList = roleList;
            ViewBag.Active = Tabs.Settings;
            var roleName = await RoleManager.FindByIdAsync(selectedRole);
            var users = selectedRole == "0" ? await UserManager.Users.ToListAsync() : await UserManager.GetUsersInRoleAsync(roleName.Name);           
            return View(users);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Lock(int id)
        {
            await _accountService.UserLockAsync(id);
            return RedirectToAction("UserList");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwitchRole (string roleName, int id)
        {
            var user = await _accountService.GetUser(id);
            if (user == null)
                return RedirectToAction("List");
            if (await UserManager.IsInRoleAsync(user, roleName))
            {
                await UserManager.RemoveFromRoleAsync(user, roleName);
            }
            else
            {
                await UserManager.AddToRoleAsync(user, roleName);

            }
            return RedirectToAction("Edit", "Account", new { id });
        }
    }
}
