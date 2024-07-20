using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Service;
using System.Security.Claims;
using System.Text;

namespace GerenciarEnderecos.Controllers
{
    public class BaseController(IJwtFunctions jwtFunctions) : Controller
    {

        public class ClaimRequirementAttribute : TypeFilterAttribute
        {
            public ClaimRequirementAttribute() : base(typeof(CustomAuthorizationFilter)) { }
        }

        public class CustomAuthorizationFilter : IAuthorizationFilter
        {

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                context.HttpContext.Session.SetString("Token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiIxIiwiZW1haWwiOiJlbWFudWVsX3hwZUBob3RtYWlsLmNvbSIsIm5iZiI6MTcyMTQ4MDkyNiwiZXhwIjoxNzIxOTEyOTI2LCJpYXQiOjE3MjE0ODA5MjZ9.VZw0mV2lpk5tuqJ6YIz2PV4wBvnKtCz4oOPeyxwQhKU");

                if (context.HttpContext.Session.GetString("Token") == null)
                    context.HttpContext.Response.Redirect("User/SignIn");
            }

            //public void OnActionExecuted(AuthorizationFilterContext context)
            //{
            //    // Do something after the action executes.
            //}
        }

        protected int Uid { get; set; }

        protected IActionResult BuildResponse(BaseResponse baseResp) => (!string.IsNullOrEmpty(baseResp.Error?.Message)) ? BadRequest(baseResp.Error.Message) : Ok(baseResp.Content);


        public override void OnActionExecuting(ActionExecutingContext context)
        {

            string token = context.HttpContext.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
            {
                int? uid = jwtFunctions.GetUidFromToken(token);

                if (uid is null)
                {
                    context.Result = new UnauthorizedObjectResult("user is unauthorized");
                    return;
                }

                Uid = uid.Value;
            }

            base.OnActionExecuting(context);
        }
    }
}
