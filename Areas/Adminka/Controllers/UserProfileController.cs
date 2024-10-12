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
using OfficePass.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace OfficePass.Area.Adminka.Controllers
{
    [Area("Adminka")]
    [Authorize(Roles = "Admin")]
    public class UserProfileController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private IUserProfileService userProfileService;
        private IGroupService groupService;
        private ISpecializationService specializationService;

        public UserProfileController(IUserService userService, IRoleService roleService, IUserProfileService userProfileService, IGroupService groupService, ISpecializationService specializationService)
        {
            this.userService = userService;
            this.roleService = roleService;
            this.userProfileService = userProfileService;
            this.groupService = groupService;
            this.specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? err)
        {
            if(err != null)
                ViewData["Error"] = err;
            var response = userProfileService.GetUserProfiles();
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var userProfileVM = new List<UserProfileViewModel>();
                foreach (var item in response.Data)
                {
                    var viewModel = new UserProfileViewModel
                    {
                        Id = item.Id,
                        FullName = $"{item.Lastname} {item.Firstname} {item.Surname}",
                        PhoneNumber = item.PhoneNumber,
                        Group = item.Group.Name,
                        Specialization = item.Specialization.Name,
                        IsBoss = item.IsBoss,
                        Login = item.User?.Login
                    };
                    userProfileVM.Add(viewModel);
                }
                return View(userProfileVM);
            }
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult AddUserProfile(string? err)
        {
            if (err != null)
                ViewData["Error"] = err;
            var responseGroup = groupService.GetGroups();
            var responseSpecialization = specializationService.GetSpecializations();
 
            if (responseGroup.Result.StatusCode == Enums.StatusCode.OK
                && responseSpecialization.StatusCode == Enums.StatusCode.OK)
            {
                 IEnumerable<Group> sourseGroup = responseGroup.Result.Data;
                SelectList selectListGroup = new SelectList(sourseGroup, "Id", "Name");

                IEnumerable<Specialization> sourseSpecialization = responseSpecialization.Data;
                SelectList selectListSpecialization = new SelectList(sourseSpecialization, "Id", "Name");

                ViewBag.SelectItemsGroup = selectListGroup;
                ViewBag.SelectItemsSpecialization = selectListSpecialization;

                var viewModel = new AddUserProfileViewModel();
                return View(viewModel);
            }
            ViewData["Error"] = "Ошибка загрузки данных";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserProfile(AddUserProfileViewModel viewModel)
        {
            var returnUrl = "/Adminka/UserProfile/AddUserProfile";
            if (ModelState.IsValid)
            {
                        var userProfile = new UserProfile()
                        {
                            Firstname = viewModel.Firstname,
                            Lastname = viewModel.Lastname,
                            Surname = viewModel.Surname,
                            PhoneNumber = viewModel.PhoneNumber,
                            SpecializationId = viewModel.SpecializationId,
                            GroupId = viewModel.GroupId,
                            IsBoss = viewModel.IsBoss,
                        };
                        var respAddUserProfile = await userProfileService.CreateUserProfile(userProfile);
                        if (respAddUserProfile.StatusCode == Enums.StatusCode.OK)
                        {
                            return RedirectToAction("Index", "UserProfile", new { err = $"Информация: {respAddUserProfile.Description}" });
                        }
                        return RedirectToAction("Index", "UserProfile", new { err = $"Ошибка: {respAddUserProfile.Description}" });
            }
            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult DeleteUserProfile(int Id)
        {
            var response = userProfileService.GetUserProfileById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new UserProfileViewModel()
                {
                    Id = response.Data.Id,
                    FullName = $"{response.Data.Lastname} {response.Data.Firstname} {response.Data.Surname}",
                    Specialization = response.Data.Specialization.Name,
                    Group = response.Data.Group.Name
                };
                return View(viewModel);
            }
            return Redirect("Index");
        }

        [HttpPost]//нужно подумать
        public IActionResult DeleteUserProfile(UserProfileViewModel userProfileModel)
        {
            var response = userProfileService.DeleteUserProfile(userProfileModel.Id);
            if (!response.Result.Data)
            {
                return RedirectToAction("Index", "UserProfile", new { err = $"Ошибка: {response.Result.Description}" });
            }
            //получаем всех юзеров из таблицы Users и Roles
            var responseUserProfile = userProfileService.GetUserProfiles();
            if (responseUserProfile.StatusCode == Enums.StatusCode.OK)
            {
                return RedirectToAction("Index", "UserProfile", new { err = $"Информация: {response.Result.Description}" });
            }
            return RedirectToAction("Index", "UserProfile", new { err = $"Ошибка: {responseUserProfile.Description}" });
        }

        [HttpGet]
        public IActionResult EditUserProfile(int Id, string? err)
        {
            if (err != null) 
               ViewData["Error"] = err;

            var response = userProfileService.GetUserProfileById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new AddUserProfileViewModel()
                {
                    Id = response.Data.Id,
                    Firstname = response.Data.Firstname,
                    Lastname = response.Data.Lastname,
                    Surname = response.Data.Surname,
                    IsBoss = response.Data.IsBoss,
                    PhoneNumber = response.Data.PhoneNumber,
                    GroupId = response.Data.GroupId,
                    SpecializationId = response.Data.SpecializationId,
                };

                var responseGroup = groupService.GetGroups();
                var responseSpecialization = specializationService.GetSpecializations();
 
                if (responseGroup.Result.StatusCode == Enums.StatusCode.OK 
                    && responseSpecialization.StatusCode == Enums.StatusCode.OK)
                {
                    IEnumerable<Group> sourseGroup = responseGroup.Result.Data;
                    SelectList selectListGroup;
                        selectListGroup = new SelectList(sourseGroup, "Id", "Name");

                    IEnumerable<Specialization> sourseSpecialization = responseSpecialization.Data;
                    SelectList selectListSpecialization;
                        selectListSpecialization = new SelectList(sourseSpecialization, "Id", "Name");

                    ViewBag.SelectItemsGroups = selectListGroup;
                    ViewBag.SelectItemsSpecialization = selectListSpecialization;
                    return View(viewModel);
                }
            }
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(int id, AddUserProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userProfile = new UserProfile()
                {
                    Id = viewModel.Id,
                    Firstname = viewModel.Firstname,
                    Lastname = viewModel.Lastname,
                    Surname = viewModel.Surname,
                    PhoneNumber = viewModel.PhoneNumber,
                    GroupId = viewModel.GroupId,
                    SpecializationId = viewModel.SpecializationId,
                    IsBoss = viewModel.IsBoss,
                };
                var responseUserProfile = await userProfileService.UpdateUserProfile(id, userProfile);

                if (responseUserProfile.StatusCode == Enums.StatusCode.OK)
                {
                    var response = userProfileService.GetUserProfiles();
                    if (response.StatusCode == Enums.StatusCode.OK)
                    {
                        return RedirectToAction("Index", "UserProfile", new { err = $"Информация: {responseUserProfile.Description}" });
                    }
                }
                return RedirectToAction("EditUserProfile", "UserProfile", new { err = $"Ошибка: {responseUserProfile.Description}" });
            }
            return RedirectToAction("EditUserProfile", "UserProfile", new { err = $"Ошибка валидации" });
        }
    }
}
