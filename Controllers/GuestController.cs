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
    public class GuestController: Controller
    {
        private IGuestService guestService;
        private ICompanyService companyService;
        private IDocumentTypeService documentTypeService;

        public GuestController(IGuestService guestService, ICompanyService companyService, IDocumentTypeService documentTypeService)
        {
            this.guestService = guestService;
            this.companyService = companyService;
            this.documentTypeService = documentTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseGuest = await guestService.GetGuests();
            if (responseGuest.StatusCode == Enums.StatusCode.OK)
            {
                var guestVM = new List<GuestViewModel>();
                foreach (var item in responseGuest.Data)
                {
                    var viewModel = new GuestViewModel
                    {
                        Id = item.Id,
                        FullName = $"{item.LastName} {item.FirstName} {item.SurName}",
                        CompanyName = item.Company.Name,
                        DocumentSerial = item.DocumentSerial,
                        DocumentNumber = item.DocumentNumber,
                        DocumentType = item.DocumentType.Name
                    };
                    guestVM.Add(viewModel);
                }

                return View(guestVM);
            }
            return RedirectToAction(HttpContext.Request.Path);
        }

        [HttpGet]
        public IActionResult AddGuest(string? err)
        {
            if (err != null)
                ViewData["Error"] = err;
            var responseCompanies = companyService.GetCompanies();
            var responseDocumentType = documentTypeService.GetDocumentTypes();
            if (responseCompanies.Result.StatusCode == Enums.StatusCode.OK
                && responseDocumentType.Result.StatusCode == Enums.StatusCode.OK)
            {
                IEnumerable<Company> sourseCompany = responseCompanies.Result.Data;
                SelectList selectListCompany = new SelectList(sourseCompany, "Id", "Name");

                IEnumerable<DocumentType> sourseDocumentType = responseDocumentType.Result.Data;
                SelectList selectListDocumentType = new SelectList(sourseDocumentType, "Id", "Name");

                ViewBag.SelectItemsCompany = selectListCompany;
                ViewBag.SelectItemsDocumentType = selectListDocumentType;

                var viewModel = new GuestViewModel();
                return View(viewModel);
            }
            ViewData["Error"] = "Ошибка загрузки данных";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGuest(GuestViewModel viewModel)
        {
            var returnUrl = "/Guest/AddGuest";
            if (ModelState.IsValid)
            {
                var respGuest = await guestService.DublicateGuest(viewModel.DocumentSerial, viewModel.DocumentSerial);
            
                var LoginId = int.Parse(HttpContext.User.Claims.FirstOrDefault(i => i.Type == "LoginId").Value);
                if (respGuest.StatusCode == Enums.StatusCode.OK)
                {
                        var guest = new Guest()
                        {
                            FirstName = viewModel.FirstName,
                            LastName = viewModel.LastName,
                            SurName = viewModel.SurName,
                            CompanyId = viewModel.CompanyId,
                            DocumentSerial = viewModel.DocumentSerial,
                            DocumentNumber = viewModel.DocumentNumber,
                            DocumentTypeId = viewModel.DocumentTypeId,
                            UserId = LoginId
                        };
                        var respAddGuest = await guestService.CreateGuest(guest);
                        if (respAddGuest.StatusCode == Enums.StatusCode.OK)
                        {
                            return RedirectToAction("Index", "Guest", new { err = $"Информация: {respAddGuest.Description}" });
                        }
                        return RedirectToAction("Index", "Guest", new { err = $"Ошибка: {respAddGuest.Description}" });
                }
                else
                {
                    return RedirectToAction("AddGuest", "Guest", new { err = $"Ошибка: {respGuest.Description}" });
                }
            }
            return Redirect(returnUrl);
        }

    }
}
