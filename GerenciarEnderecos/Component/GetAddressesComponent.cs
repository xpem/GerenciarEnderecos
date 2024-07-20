using Microsoft.AspNetCore.Mvc;
using Service;

namespace GerenciarEnderecos.Component
{
    public class GetAddressesComponent(IAddressService addressService, IJwtFunctions jwtFunctions) : ViewComponent
    {
        public async Task<IViewComponentResult> GetAddresses()
        {

            string token = HttpContext.Session.GetString("Token");
            int? uid = jwtFunctions.GetUidFromToken(token);

            List<Domain.DTOs.Address> resp = await addressService.GetAsync(uid.Value);
            return View(resp);
        }
    }
}
