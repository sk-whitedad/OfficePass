using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OfficePass.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

    }
}
