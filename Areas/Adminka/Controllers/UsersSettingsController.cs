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

namespace OfficePass.Area.Adminka.Controllers
{
    [Area("Adminka")]
    [Authorize(Roles = "Admin")]
    public class UsersSettingsController : Controller
    {
        private readonly IUserService usersSettingsService;
        private readonly IRoleService roleService;
        private IUserProfileService userProfileService;

        public UsersSettingsController(IUserService usersSettingsService, IRoleService roleService, IUserProfileService userProfileService)
        {
            this.usersSettingsService = usersSettingsService;
            this.roleService = roleService;
            this.userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //получаем всех юзеров из таблицы Users и Roles
            var responseUsersSettings = usersSettingsService.GetUsers();
            if (responseUsersSettings.Result.StatusCode == Enums.StatusCode.OK)
            {
                return View(responseUsersSettings.Result.Data);

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            var responceRole = roleService.GetRoles();
            if (responceRole.StatusCode == Enums.StatusCode.OK)
            {
                List<string> sourse = new List<string>();
                foreach (var item in responceRole.Data)
                {
                    sourse.Add(item.Name);
                }

                SelectList selectList = new SelectList(sourse, sourse[1]);
                ViewBag.SelectItems = selectList;
                return View();
            }
            ViewData["Error"] = "Ошибка загрузки ролей";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string SelectedItem, AddUserViewModel viewModel)
        {
            //получаем список ролей
            var responceRole = roleService.GetRoles();
            if (responceRole.StatusCode == Enums.StatusCode.OK 
                && viewModel.Password == viewModel.ConfirmPassword)
            {
                var role = responceRole.Data.FirstOrDefault(x => x.Name == SelectedItem);
                var userModel = new User()
                {
                    Login = viewModel.Login,
                    Password = HashPasswordHelper.HashPassowrd(viewModel.Password),
                    Role = role,
                    UserProfile = viewModel.UserProfile,
                };
                 await usersSettingsService.CreateUser(userModel);
            }
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult DeleteUser(int Id)
        {
            var userResponse = usersSettingsService.GetUserById(Id);
            if (userResponse.StatusCode == Enums.StatusCode.OK)
            {
                return View(userResponse.Data);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult DeleteUser(User userModel)
        {
            var userResponse = usersSettingsService.DeleteUser(userModel.Id);
            if (!userResponse.Result.Data)
            {
                ViewData["Title"] = "Пользователь не удален";
            }
            //получаем всех юзеров из таблицы Users и Roles
            var responseUsersSettings = usersSettingsService.GetUsers();
            if (responseUsersSettings.Result.StatusCode == Enums.StatusCode.OK)
            {
                return View("Index", responseUsersSettings.Result.Data);

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditUser(int Id)
        {
            var userResponse = usersSettingsService.GetUserById(Id);
            if (userResponse.StatusCode == Enums.StatusCode.OK)
            {
                var userViewModel = new AddUserViewModel()
                {
                    Id = Id,
                    Login = userResponse.Data.Login,
                    UserProfile = userResponse.Data.UserProfile
                };
                return View(userViewModel);
            }
            return Redirect("Index");

        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AddUserViewModel viewModel)
        {
            var user = new User()
            {
                Id = viewModel.Id,
                Login = viewModel.Login,
                UserProfile = viewModel.UserProfile,
            };
            if (!String.IsNullOrEmpty(viewModel.Password))
                user.Password = HashPasswordHelper.HashPassowrd(viewModel.Password);

                var responseUpdateUser = await usersSettingsService.UpdateUser(user);
            var responseUpdateProfole = await userProfileService.UpdateUserProfile(viewModel.Id, viewModel.UserProfile);

            if (responseUpdateUser.StatusCode == Enums.StatusCode.OK
                && responseUpdateProfole.StatusCode == Enums.StatusCode.OK)
            {
                //получаем всех юзеров из таблицы Users и Roles
                var responseUsersSettings = usersSettingsService.GetUsers();
                if (responseUsersSettings.Result.StatusCode == Enums.StatusCode.OK)
                {
                    return View("Index", responseUsersSettings.Result.Data);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DetailsUser(int id)
        {
            var userResponse = usersSettingsService.GetUserById(id);
            if (userResponse.StatusCode == Enums.StatusCode.OK)
            {
                return View(userResponse.Data);
            }
            return Redirect("Index");
        }

    }
}
