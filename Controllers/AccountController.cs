using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficePass.Models;
using OfficePass.Services;
using System.Diagnostics;
using System.Security.Claims;
using OfficePass.Enums;

namespace OfficePass.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService loginService;

        public AccountController(ILoginService loginService)
        {
            if (loginService == null) throw new ArgumentNullException(nameof(loginService));
            this.loginService = loginService;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var returnUrl = "/Account/Index";
            if (ModelState.IsValid)
            {
                var response = await loginService.Login(model);
                if (response.StatusCode == Enums.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(response.Data));
                    returnUrl = "/Home/Index";
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", response.Description);
            }
            return Redirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
