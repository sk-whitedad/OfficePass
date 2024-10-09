using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficePass.Domain.Entities;
using OfficePass.Models;
using OfficePass.Services;

namespace OfficePass.Controllers
{
    [Authorize]
    public class GroupController: Controller
    {
        private IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseGroup = await groupService.GetGroups();
            if (responseGroup.StatusCode == Enums.StatusCode.OK)
            {
                return View(responseGroup.Data);
            }
            return RedirectToAction(HttpContext.Request.Path);
        }

        [HttpGet]
        public IActionResult AddGroup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(GroupViewModel viewModel)
        {
            var model = new Group()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
            };
            await groupService.CreateGroup(model);
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult DeleteGroup(int Id)
        {
            var groupResponse = groupService.GetGroupById(Id);
            if (groupResponse.StatusCode == Enums.StatusCode.OK)
            {
                return View(groupResponse.Data);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult DeleteGroup(Group model)
        {
            var groupResponse = groupService.DeleteGroup(model.Id);
            if (!groupResponse.Result.Data)
            {
                ViewData["Title"] = "Компания не удалена";
            }
            var responseGroup = groupService.GetGroups();
            if (responseGroup.Result.StatusCode == Enums.StatusCode.OK)
            {
                return View("Index", responseGroup.Result.Data);

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditGroup(int Id)
        {
            var groupResponse = groupService.GetGroupById(Id);
            if (groupResponse.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new GroupViewModel()
                {
                    Id = Id,
                    Name = groupResponse.Data.Name,
                    Description = groupResponse.Data.Description,
                };
                return View(viewModel);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(GroupViewModel viewModel)
        {
            var group = new Group()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
            };
            var responseUpdateGroup = await groupService.UpdateGroup(group);

            if (responseUpdateGroup.StatusCode == Enums.StatusCode.OK)
            {
                var responseGroup = await groupService.GetGroups();
                if (responseGroup.StatusCode == Enums.StatusCode.OK)
                {
                    return View("Index", responseUpdateGroup.Data);
                }
            }
            return RedirectToAction("Index", "Home");
        }



    }
}
