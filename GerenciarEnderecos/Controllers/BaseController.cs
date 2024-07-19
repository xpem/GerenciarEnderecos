using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace GerenciarEnderecos.Controllers
{
    public class BaseController : Controller
    {

        public class ClaimRequirementAttribute : TypeFilterAttribute
        {
            public ClaimRequirementAttribute() : base(typeof(CustomAuthorizationFilter)) { }
        }

        public class CustomAuthorizationFilter : IAuthorizationFilter
        {
            public void OnAuthorization(AuthorizationFilterContext context)
            {
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

        protected int? RecoverUidSession()
        {
            string? uid = null;

            if (HttpContext.User.Identity is ClaimsIdentity identity)
                uid = identity.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;

            return uid != null ? Convert.ToInt32(uid) : null;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Microsoft.Extensions.Primitives.StringValues auth = context.HttpContext.Request.Headers.Authorization;
            if (!string.IsNullOrEmpty(auth))
            {
                int? uid = RecoverUidSession();

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
