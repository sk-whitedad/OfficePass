using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OfficePass.Areas.Adminka.Models;
using OfficePass.Areas.Adminka.Services;
using OfficePass.Domain.Entities;

namespace OfficePass.Area.Adminka.Controllers
{
    [Area("Adminka")]
    [Authorize(Roles = "Admin")]
    public class SpecializationController : Controller
    {
        private ISpecializationService specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            this.specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? err)
        {
            if (err != null)
                ViewData["Error"] = err;
            var response = specializationService.GetSpecializations();
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var specializationVM = new List<SpecializationViewModel>();
                foreach (var item in response.Data)
                {
                    var viewModel = new SpecializationViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                    };
                    specializationVM.Add(viewModel);
                }
                return View(specializationVM);
            }
            return Redirect("~/Home/Index");
        }

        [HttpGet]
        public IActionResult AddSpecialization(string? err)
        {
            if (err != null)
                ViewData["Error"] = err;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialization(SpecializationViewModel viewModel)
        {
            var returnUrl = "/Adminka/UserProfile/AddGroup";
            if (ModelState.IsValid)
            {
                var specialization = new Specialization()
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                };
                var respAddSpecialization = await specializationService.CreateSpecialization(specialization);
                if (respAddSpecialization.StatusCode == Enums.StatusCode.OK)
                {
                    return RedirectToAction("Index", "Specialization", new { err = $"Информация: {respAddSpecialization.Description}" });
                }
                return RedirectToAction("Index", "UserProfile", new { err = $"Ошибка: {respAddSpecialization.Description}" });
            }
            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult DeleteSpecialization(int Id)
        {
            var response = specializationService.GetSpecializationById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new SpecializationViewModel()
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
        public IActionResult DeleteSpecialization(SpecializationViewModel specializationModel)
        {
            var response = specializationService.DeleteSpecialization(specializationModel.Id);
            if (!response.Result.Data)
            {
                return RedirectToAction("Index", "Specialization", new { err = $"Ошибка: {response.Result.Description}" });
            }
            var responseSpecialization = specializationService.GetSpecializations();
            if (responseSpecialization.StatusCode == Enums.StatusCode.OK)
            {
                return RedirectToAction("Index", "Specialization", new { err = $"Информация: {response.Result.Description}" });
            }
            return RedirectToAction("Index", "Specialization", new { err = $"Ошибка: {responseSpecialization.Description}" });
        }

        [HttpGet]
        public IActionResult EditSpecialization(int Id, string? err)
        {
            if (err != null)
                ViewData["Error"] = err;

            var response = specializationService.GetSpecializationById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new SpecializationViewModel()
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
        public async Task<IActionResult> EditSpecialization(SpecializationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new Specialization()
                {
                    Id = viewModel.Id,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                };

                var responseSpecialization = await specializationService.UpdateSpecialization(model);

                if (responseSpecialization.StatusCode == Enums.StatusCode.OK)
                {
                    var response = specializationService.GetSpecializations();
                    if (response.StatusCode == Enums.StatusCode.OK)
                    {
                        return RedirectToAction("Index", "Specialization", new { err = $"Информация: {responseSpecialization.Description}" });
                    }
                }
                return RedirectToAction("EditSpecialization", "Specialization", new { err = $"Ошибка: {responseSpecialization.Description}" });
            }
            return RedirectToAction("EditSpecialization", "Specialization", new { err = $"Ошибка валидации" });
        }

        public IActionResult DetailsModal(int id)
        {
            var s = specializationService.GetSpecializationById(id).Data;
            if (s != null)
                return PartialView(s);
            return null;
        }


    }
}
