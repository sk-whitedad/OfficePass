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
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private IUserProfileService userProfileService;
        private IGroupService groupService;
        private ISpecializationService specializationService;

        public UserController(IUserService userService, IRoleService roleService, IUserProfileService userProfileService, IGroupService groupService, ISpecializationService specializationService)
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
            //получаем всех юзеров из таблицы Users и Roles
            var response = userService.GetUsers();
            if (response.Result.StatusCode == Enums.StatusCode.OK)
            {
                var userViewModels = new List<UserViewModel>();
                foreach (var item in response.Result.Data)
                {
                    var viewModel = new UserViewModel
                    {
                        Id = item.Id,
                        Login = item.Login,
                        Role = item.Role,
                        UserProfile = $"{item.UserProfile.Lastname} {item.UserProfile.Firstname[0]}.{item.UserProfile.Surname[0]}."
                    };
                    userViewModels.Add(viewModel);
                }
                return View(userViewModels);
            }
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult AddUser(string? err)
        {
            if (err != null)
                ViewData["Error"] = err;
            var responseRole = roleService.GetRoles();
            var responseGroup = groupService.GetGroups();
            var responseSpecialization = specializationService.GetSpecializations();
            var responseUserProfile = userProfileService.GetUserProfiles();

            if (responseRole.StatusCode == Enums.StatusCode.OK 
                && responseGroup.Result.StatusCode == Enums.StatusCode.OK
                && responseSpecialization.StatusCode == Enums.StatusCode.OK
                && responseUserProfile.StatusCode == Enums.StatusCode.OK)
            {
                IEnumerable<Role> sourseRole = responseRole.Data;

                SelectList selectListRole = new SelectList(sourseRole, "Id", "Name");

                IEnumerable<Group> sourseGroup = responseGroup.Result.Data;
                SelectList selectListGroup = new SelectList(sourseGroup, "Id", "Name");

                IEnumerable<Specialization> sourseSpecialization = responseSpecialization.Data;
                SelectList selectListSpecialization = new SelectList(sourseSpecialization, "Id", "Name");

                IEnumerable<UserProfile> sourseUserProfile = responseUserProfile.Data;
                foreach (var item in sourseUserProfile)
                {
                    item.Firstname = $"{item.Lastname} {item.Firstname} {item.Surname}";
                }
                SelectList selectListUserProfile = new SelectList(sourseUserProfile, "Id", "Firstname");

                ViewBag.SelectItemsRole = selectListRole;
                ViewBag.SelectItemsGroup = selectListGroup;
                ViewBag.SelectItemsSpecialization = selectListSpecialization;
                ViewBag.SelectItemsUserProfile = selectListUserProfile;

                var viewModel = new AddUserViewModel();
                return View(viewModel);
            }
            ViewData["Error"] = "Ошибка загрузки данных";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel viewModel)
        {
            var returnUrl = "/Adminka/User/AddUser";
            if (ModelState.IsValid)
            {
                var respUser = await userService.DublicateUser(viewModel.Login);
                if(respUser.StatusCode == Enums.StatusCode.OK)
                {
                    var respUserProfileDuble = await userService.DublicateUserProfile(viewModel.UserProfileId);
                    if (respUserProfileDuble.Data)
                    {
                        var user = new User()
                        {
                            Login = viewModel.Login,
                            Password = HashPasswordHelper.HashPassowrd(viewModel.Password),
                            RoleId = viewModel.RoleId,
                            UserProfileId = viewModel.UserProfileId,
                            
                        };
                        var respAddUser = await userService.CreateUser(user);
                        if (respAddUser.StatusCode == Enums.StatusCode.OK)
                        {
                            return RedirectToAction("Index", "User", new { err = $"Информация: {respAddUser.Description}" });
                        }
                        return RedirectToAction("Index", "User", new { err = $"Ошибка: {respAddUser.Description}" });
                    }
                    else
                    {
                        return RedirectToAction("AddUser", "User", new { err = $"Ошибка: {respUserProfileDuble.Description}" });
                    }
                }
                else
                {
                    return RedirectToAction("AddUser", "User", new { err = $"Ошибка: {respUser.Description}" });
                }
            }
            return Redirect(returnUrl);
        }

        [HttpGet]//нужно подумать
        public IActionResult DeleteUser(int Id)
        {
            var response = userService.GetUserById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                return View(response.Data);
            }
            return Redirect("Index");
        }

        [HttpPost]//нужно подумать
        public IActionResult DeleteUser(User userModel)
        {
            var response = userService.DeleteUser(userModel.Id);
            if (!response.Result.Data)
            {
                ViewData["Title"] = "Пользователь не удален";
            }
            //получаем всех юзеров из таблицы Users и Roles
            var responseUsers = userService.GetUsers();
            if (responseUsers.Result.StatusCode == Enums.StatusCode.OK)
            {
                return RedirectToAction("Index", "User", new { err = $"Информация: {response.Result.Description}" });
            }
            return RedirectToAction("Index", "User", new { err = $"Ошибка: {responseUsers.Result.Description}" });
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

        [HttpGet]
        public IActionResult DetailsUser(int id)
        {
            var response = userService.GetUserById(id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new DetailUserViewModel
                {
                    Id = response.Data.Id,
                    Login = response.Data.Login,
                    UserProfile = response.Data.UserProfile,
                    Role = response.Data.Role
                };
                return View(viewModel);
            }
            return Redirect("Index");
        }
    }
}
