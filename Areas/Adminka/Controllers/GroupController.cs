using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficePass.Areas.Adminka.Models;
using OfficePass.Areas.Adminka.Services;
using OfficePass.Enums;
using OfficePass.Domain.Entities;
using OfficePass.Helpers;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficePass.Services;

namespace OfficePass.Area.Adminka.Controllers
{
    [Area("Adminka")]
    [Authorize(Roles = "Admin")]
    public class GroupController : Controller
    {
        private IGroupService groupService;
        
        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? err)
        {
            if(err != null)
                ViewData["Error"] = err;
            var response = await groupService.GetGroups();
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var groupVM = new List<GroupViewModel>();
                foreach (var item in response.Data)
                {
                    var viewModel = new GroupViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                    };
                    groupVM.Add(viewModel);
                }
                return View(groupVM);
            }
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult AddGroup(string? err)
        {
            if (err != null)
                ViewData["Error"] = err;
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(GroupViewModel viewModel)
        {
            var returnUrl = "/Adminka/UserProfile/AddGroup";
            if (ModelState.IsValid)
            {
                        var group = new Group()
                        {
                            Name = viewModel.Name,
                            Description = viewModel.Description,
                        };
                        var respAddGroup = await groupService.CreateGroup(group);
                        if (respAddGroup.StatusCode == Enums.StatusCode.OK)
                        {
                            return RedirectToAction("Index", "Group", new { err = $"Информация: {respAddGroup.Description}" });
                        }
                        return RedirectToAction("Index", "UserProfile", new { err = $"Ошибка: {respAddGroup.Description}" });
            }
            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult DeleteGroup(int Id)
        {
            var response = groupService.GetGroupById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new GroupViewModel()
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    Description = response.Data.Description,
                };
                return View(viewModel);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult DeleteGroup(GroupViewModel groupModel)
        {
            var response = groupService.DeleteGroup(groupModel.Id);
            if (!response.Result.Data)
            {
                return RedirectToAction("Index", "Group", new { err = $"Ошибка: {response.Result.Description}" });
            }
            var responseGroup = groupService.GetGroups();
            if (responseGroup.Result.StatusCode == Enums.StatusCode.OK)
            {
                return RedirectToAction("Index", "Group", new { err = $"Информация: {response.Result.Description}" });
            }
            return RedirectToAction("Index", "Group", new { err = $"Ошибка: {responseGroup.Result.Description}" });
        }

        [HttpGet]
        public IActionResult EditGroup(int Id, string? err)
        {
            if (err != null)
                ViewData["Error"] = err;

            var response = groupService.GetGroupById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new GroupViewModel()
                {
                    Id = response.Data.Id,
                    Name = response.Data.Name,
                    Description = response.Data.Description,
                };
                    return View(viewModel);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(GroupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var group = new Group()
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                };

                var responseGroup = await groupService.UpdateGroup(group);

                if (responseGroup.StatusCode == Enums.StatusCode.OK)
                {
                    var response = await groupService.GetGroups();
                    if (response.StatusCode == Enums.StatusCode.OK)
                    {
                        return RedirectToAction("Index", "Group", new { err = $"Информация: {responseGroup.Description}" });
                    }
                }
                return RedirectToAction("EditGroup", "Group", new { err = $"Ошибка: {responseGroup.Description}, {responseGroup.Description}" });
            }
            return RedirectToAction("EditGroup", "Group", new { err = $"Ошибка валидации" });
        }

    }
}
