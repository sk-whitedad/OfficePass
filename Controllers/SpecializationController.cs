using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficePass.Areas.Adminka.Models;
using OfficePass.Areas.Adminka.Services;
using OfficePass.Domain.Entities;
using OfficePass.Helpers;
using OfficePass.Models;
using OfficePass.Services;

namespace OfficePass.Controllers
{
    [Authorize]
    public class SpecializationController : Controller
    {
        private ISpecializationService specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            this.specializationService = specializationService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var responseSpecialization = specializationService.GetSpecializations();
            if (responseSpecialization.StatusCode == Enums.StatusCode.OK)
            {
                return View(responseSpecialization.Data);
            }
            return RedirectToAction(HttpContext.Request.Path);
        }

        [HttpGet]
        public IActionResult AddSpecialization()
        {
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSpecialization(SpecializationViewModel viewModel)
        {
                var model = new Specialization()
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                };
                await specializationService.CreateSpecialization(model);
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult DeleteSpecialization(int Id)
        {
            var specializationResponce = specializationService.GetSpecializationById(Id);
            if (specializationResponce.StatusCode == Enums.StatusCode.OK)
            {
                return View(specializationResponce.Data);
            }
            return Redirect("Index");
        }

        [HttpPost]
        public IActionResult DeleteSpecialization(Specialization specialization)
        {
            var specializationResponce = specializationService.DeleteSpecialization(specialization.Id);
            if (!specializationResponce.Result.Data)
            {
                ViewData["Title"] = "Должность не удалена";
            }
            var response = specializationService.GetSpecializations();
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                return View("Index", response.Data);

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult EditSpecialization(int Id)
        {
            var response = specializationService.GetSpecializationById(Id);
            if (response.StatusCode == Enums.StatusCode.OK)
            {
                var viewModel = new SpecializationViewModel()
                {
                    Id = Id,
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
            var specialization = new Specialization()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
            };
            var responseSpecialization = await specializationService.UpdateSpecialization(specialization);

            if (responseSpecialization.StatusCode == Enums.StatusCode.OK)
            {
                var response = specializationService.GetSpecializations();
                if (response.StatusCode == Enums.StatusCode.OK)
                {
                    return View("Index", response.Data);
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
