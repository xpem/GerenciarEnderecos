using Domain.Requests;
using GerenciarEnderecos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Service;
using System.Diagnostics;
using System.Security.Claims;

namespace GerenciarEnderecos.Controllers
{

    public class HomeController(IAddressService addressService, IJwtFunctions jwtFunctions) : BaseController(jwtFunctions)
    {

        [ClaimRequirementAttribute]
        public async Task<IActionResult> Index()
        {
            List<Domain.DTOs.Address> resp = await addressService.GetAsync(Uid);
            return View(resp);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(AddressRequest request)
        {
            var resp = await addressService.CreateAsync(request, Uid);
            ViewBag.Address = JsonConvert.SerializeObject(resp);
            TempData["Success"] = "Endereço cadastrado!";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = addressService.GetByIdAsync(id.Value, Uid);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
