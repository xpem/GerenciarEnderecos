using Domain.Requests.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Service;

namespace GerenciarEnderecos.Controllers
{
    public class UserController(IUserService userService, IJwtFunctions jwtFunctions) : BaseController(jwtFunctions)
    {

        public IActionResult SignUp() => View();

        public IActionResult SignIn() => View();

        [HttpPost]
        public async Task<IActionResult> SignUp(UserRequest reqUser)
        {
            var resp = await userService.CreateAsync(reqUser);

            if (!resp.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            else return View(BuildResponse(resp));
        }

        //[Route("Session")]
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSessionRequest reqUserSession)
        {
            var resp = await userService.GenerateUserTokenAsync(reqUserSession);
            if (resp.Content != null)
            {
                HttpContext.Session.SetString("Token", resp.Content as string);

                return RedirectToAction("Index", "Home");
            }
            else return View(resp);
        }

        [Route("SignOut")]
        [HttpGet]
        public async Task<IActionResult> SignOutAsync()
        {
            HttpContext.Session.Remove("Token");
            return View("SignIn", "User");
        }
    }
}
