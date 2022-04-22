using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LearnJWTAuthentication.Models;
using LearnJWTAuthentication.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LearnJWTAuthentication.Middlewares
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appsettings;

        public JWTMiddleware(RequestDelegate next, IOptions<AppSettings> appsettings)
        {
            _next = next;
            _appsettings = appsettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserServices userServices)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(' ').Last();

            if(token != null)
            {
                AuthenticateAndAttachUser(context, userServices, token);
            }

            _next(context);
        }

        private void AuthenticateAndAttachUser(HttpContext context, IUserServices userServices, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appsettings.JWTSecret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters(){
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ClockSkew = TimeSpan.Zero
            },out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Int32.Parse(jwtToken.Claims.First( c => c.Type == "ID").Value);

            context.Items["User"] = userServices.GetUser(userId);
        }
    }
}