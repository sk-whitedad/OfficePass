﻿using Microsoft.AspNetCore.Authorization;
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
    public class CompanyController: Controller
    {
        private ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var responseCompany = await companyService.GetCompanies();
            if (responseCompany.StatusCode == Enums.StatusCode.OK)
            {
                return View(responseCompany.Data);
            }
            return RedirectToAction(HttpContext.Request.Path);
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCompany(CompanyViewModel viewModel)
        {
                var model = new Company()
                {
                    Name = viewModel.Name,
                    Address = viewModel.Address,
                    PhoneNumber = viewModel.PhoneNumber,
                };
                await companyService.CreateCompany(model);
            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult DeleteCompany(int Id)
        {
            var companyResponse = companyService.GetCompanyById(Id);
            if (companyResponse.StatusCode == Enums.StatusCode.OK)
            {
                return View(companyResponse.Data);
            }
            return Redirect("Index");
        }

       
        public IActionResult DeleteCompany(Company company)
        {
            var companyResponse = companyService.DeleteCompany(company.Id);
            if (!companyResponse.Result.Data)
            {
                ViewData["Title"] = "Компания не удалена";
            }
            var responseCompany = companyService.GetCompanies();
            if (responseCompany.Result.StatusCode == Enums.StatusCode.OK)
            {
                return View("Index", responseCompany.Result.Data);

            }
            return RedirectToAction("Index", "Home");
        }

    }
}