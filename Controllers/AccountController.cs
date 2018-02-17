using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using ASP.Models.ViewModels;
using ASP.Models;
using System.Security.Claims;

namespace ASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBContext db = new DBContext();
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            { 
                return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var account =  db.GetAccount(loginModel.Name, loginModel.Password);
                if (account.Name == loginModel.Name && account.Password == loginModel.Password)
                {
                    await Authenticate(loginModel.Name);
                    return Redirect("/Home/Index");
                }
                return View();
            }
            ModelState.AddModelError("", "Invalid name or password");
            return View();
        }


        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };
            await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    new ClaimsPrincipal(claimsIdentity),
    authProperties);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await HttpContext.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}