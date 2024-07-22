using CsvHelper;
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
using System.Formats.Asn1;
using System.Security.Claims;
using CsvHelper.Configuration;
using System.Text;

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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await addressService.GetByIdAsync(id.Value, Uid);
            if (address == null)
            {
                return NotFound();
            }

            AddressRequest addressRequest = new()
            {
                CEP = address.CEP,
                City = address.City,
                Neighborhood = address.Neighborhood,
                Number = address.Number.ToString(),
                State = address.State,
                Street = address.Street,
                Complement = address.Complement
            };

            return View(addressRequest);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AddressRequest request)
        {
            var resp = await addressService.UpdateAsync(request, Uid);

            ViewBag.Address = JsonConvert.SerializeObject(resp);
            TempData["Success"] = "Endereço Alterado!";
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await addressService.GetByIdAsync(id.Value, Uid);
            if (address == null)
            {
                return NotFound();
            }

            AddressRequest addressRequest = new() { Id = address.Id, CEP = address.CEP, City = address.City, Neighborhood = address.Neighborhood, Number = address.Number.ToString(), State = address.State, Street = address.Street, Complement = address.Complement };

            return View(addressRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int? id)
        {
            var address = await addressService.GetByIdAsync(id.Value, Uid);
            if (address == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await addressService.DeleteAddress(id.Value, Uid);

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToCsv()
        {
            List<Domain.DTOs.Address> addresses = await addressService.GetAsync(Uid);

            List<AddressRequest> addressesRequest = new List<AddressRequest>();

            foreach (var address in addresses)
            {
                addressesRequest.Add(new()
                {
                    CEP = address.CEP,
                    City = address.City,
                    Neighborhood = address.Neighborhood,
                    Number = address.Number.ToString(),
                    State = address.State,
                    Street = address.Street,
                    Complement = address.Complement,
                    Id = address.Id
                });
            }

            var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
            using var ms = new MemoryStream();
            using var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true));
            using (var cw = new CsvWriter(sw, cc))
            {
                cw.WriteRecords(addressesRequest);
            }// The stream gets flushed here.
            return File(ms.ToArray(), "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
        }


    }
}
