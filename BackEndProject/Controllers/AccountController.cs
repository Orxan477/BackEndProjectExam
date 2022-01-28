using BackEndProject.Utilities;
using BackEndProject.ViewModels.Account;
using Core.Models;
using Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BackEndProject.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager;
        private IConfiguration _configure;
        private RoleManager<IdentityRole> _roleManager;
        private AppDbContext _context;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,
                                 IConfiguration configure,
                                 RoleManager<IdentityRole> roleManager,
                                 AppDbContext context,
                                 SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _configure = configure;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            IsAuthenticated();
            if (!ModelState.IsValid) return View(register);
            AppUser user = new AppUser
            {
                Email = register.Email,
                UserName = register.Username,
                FullName = register.FullName
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, register.Password);
            if (identityResult.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());
                string body = $"<a href=\"{link}\">Verify Email</a>";
                Email.Send(_configure.GetSection("EmailSettings:Mail").Value,
                           _configure.GetSection("EmailSettings:Passowrd").Value, user.Email, body, "ConfirmationLink");
                await _userManager.AddToRoleAsync(user, "Admin");
                //await _userManager.AddToRoleAsync(user, "Member");
                ViewBag.IsSuccess = true;
                return View();
            }
            else
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(string userid,string token)
        {
            var user =await _userManager.FindByIdAsync(userid);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.Confirm = true;
                user.IsActivated = true;
                await _context.SaveChangesAsync();
                return View(nameof(Login));
            }
            else return BadRequest();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm login)
        {
            IsAuthenticated();
            if (!ModelState.IsValid) return View(login);
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null) return NotFound();
            //if (!user.IsActivated)
            //{
            //    ModelState.AddModelError("", "Please Confirm Email");
            //    return View();
            //}
            var result =await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your Account is Locked. Please wait");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Please Confirm Email");
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRoles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                   await _roleManager.CreateAsync(new IdentityRole {Name=item.ToString() });
                }
            }
            return Content("Ok");
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private void IsAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                throw new System.Exception("You Alredy Authenticated");
            }
        }
    }
}
