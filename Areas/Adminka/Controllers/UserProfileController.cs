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
        public IActionResult EditUser(int Id, string? err)
        {
            if (err != null) 
               ViewData["Error"] = err;

            var response = userService.GetUserById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new EditUserViewModel()
                {
                    Id = response.Data.Id,
                    Login = response.Data.Login,
                    UserProfileId = response.Data.UserProfile.Id,
                    Lastname = response.Data.UserProfile.Lastname,
                    Firstname = response.Data.UserProfile.Firstname,
                    Surname = response.Data.UserProfile.Surname,
                    RoleId = response.Data.RoleId,
                    GroupId = response.Data.UserProfile.GroupId,
                    SpecializationId = response.Data.UserProfile.SpecializationId,
                    IsBoss = response.Data.UserProfile.IsBoss,
                };

                var responseGroup = groupService.GetGroups();
                var responseSpecialization = specializationService.GetSpecializations();
                var responseRole = roleService.GetRoles();
 
                if (responseGroup.Result.StatusCode == Enums.StatusCode.OK && responseSpecialization.StatusCode == Enums.StatusCode.OK)
                {
                    IEnumerable<Group> sourseGroup = responseGroup.Result.Data;
                    SelectList selectListGroup;
                    if (response.Data.UserProfile.Group != null)
                        selectListGroup = new SelectList(sourseGroup, "Id", "Name");
                    else
                        selectListGroup = new SelectList(sourseGroup);

                    IEnumerable<Specialization> sourseSpecialization = responseSpecialization.Data;
                    SelectList selectListSpecialization;
                    if (response.Data.UserProfile.Specialization != null)
                        selectListSpecialization = new SelectList(sourseSpecialization, "Id", "Name");
                    else
                        selectListSpecialization = new SelectList(sourseSpecialization);

                    IEnumerable<Role> sourseRole = responseRole.Data;
                    SelectList selectListRole;
                    if (response.Data.Role != null)
                        selectListRole = new SelectList(sourseRole, "Id", "Name");
                    else
                        selectListRole = new SelectList(sourseRole);

                    ViewBag.SelectItemsGroups = selectListGroup;
                    ViewBag.SelectItemsSpecialization = selectListSpecialization;
                    ViewBag.SelectItemsRole = selectListRole;
                    return View(viewModel);
                }
            }
            return Redirect("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Id = viewModel.Id,
                    Login = viewModel.Login,
                    RoleId = viewModel.RoleId,
                };
                if (!String.IsNullOrEmpty(viewModel.Password))
                    user.Password = HashPasswordHelper.HashPassowrd(viewModel.Password);

                var responseUser = await userService.UpdateUser(user);

                var userProfile = new UserProfile()
                {
                    Firstname = viewModel.Firstname,
                    Lastname = viewModel.Lastname,
                    Surname = viewModel.Surname,
                    IsBoss = viewModel.IsBoss,
                    GroupId = viewModel.GroupId,
                    SpecializationId = viewModel.SpecializationId,
                };
                var responseProfile = await userProfileService.UpdateUserProfile(viewModel.UserProfileId, userProfile);

                if (responseUser.StatusCode == Enums.StatusCode.OK
                    && responseProfile.StatusCode == Enums.StatusCode.OK)
                {
                    //получаем всех юзеров из таблицы Users и Roles
                    var response = await userService.GetUsers();
                    if (response.StatusCode == Enums.StatusCode.OK)
                    {
                        return RedirectToAction("Index", "User", new { err = $"Информация: {responseUser.Description}" });
                    }
                }
                return RedirectToAction("EditUser", "User", new { err = $"Ошибка: {responseUser.Description}, {responseProfile.Description}" });
            }
            return RedirectToAction("EditUser", "User", new { err = $"Ошибка валидации: Пароли не совпадают" });
        }
    }
}
