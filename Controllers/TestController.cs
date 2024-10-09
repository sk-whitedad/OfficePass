using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficePass.Areas.Adminka.Models;
using OfficePass.Domain.Entities;

namespace OfficePass.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TestStart()
        {
            IEnumerable<Role> modelRoles = new List<Role>
            {
                new Role{Id = 1, Name = "Admin"},
                new Role{Id = 2, Name = "Moder"},
                new Role{Id = 3, Name = "User"},
            };

            ViewBag.Roles = new SelectList(modelRoles, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult TestStart(AddUserViewModel addUserViewModel)
        {
            var role = addUserViewModel.RoleId;
            return View("Index");
        }

    }
}
