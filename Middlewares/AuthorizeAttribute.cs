using LearnJWTAuthentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearnJWTAuthentication.Middlewares
{
    [AttributeUsage(AttributeTargets.All)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (UserEntity)context.HttpContext.Items["User"];

            if(user == null)
            {
                context.Result = new JsonResult(new {Message = "Unauthorized access"}){StatusCode = StatusCodes.Status401Unauthorized};
            }
        }
    }
}