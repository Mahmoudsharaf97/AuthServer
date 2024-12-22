using Auth_Core.Global;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Auth_Application.Attributes
{
    public  class AppAuthorize : Attribute , IAsyncAuthorizationFilter
    {
      
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var lang = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "lang").Value;
            var channel = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "channel").Value;
            var userName = context.HttpContext.User.Identity?.Name;
        //    var token = context.HttpContext.Request.Headers.Authorization.ToString().Substring(7);
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var anynmous = jwt.Claims.Where(x => x.Type == "Any").FirstOrDefault()?.Value;
            if (!string.IsNullOrEmpty(anynmous) && anynmous.ToLower()=="yes")
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var sessionId = jwt.Claims.Where(x => x.Type == "SessionId").FirstOrDefault()?.Value;
            var userid = jwt.Claims.Where(x => x.Type == "UserId").FirstOrDefault().Value.ToString();
            var username = jwt.Claims.Where(x => x.Type == "UserName").FirstOrDefault()?.Value;
            var email = jwt.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value;
            var LockoutEnabled = jwt.Claims.Where(x => x.Type == "LockoutEnabled").FirstOrDefault()?.Value;
            var LockoutEnd = jwt.Claims.Where(x => x.Type == "LockoutEnd").FirstOrDefault()?.Value;
            if (string.IsNullOrEmpty(sessionId))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            var EmailConfirmed = jwt.Claims.Where(x => x.Type == "EmailConfirmed").FirstOrDefault()?.Value;
            // set global info
            var globalInfo = context.HttpContext.RequestServices.GetService<GlobalInfo>();
            globalInfo.SetValues(username,userid, lang, channel, email, null, "", "", LockoutEnabled, LockoutEnd);
    
            return;
        }

    }
}
