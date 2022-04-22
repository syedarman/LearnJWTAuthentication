using System.Net;
using LearnJWTAuthentication.Models;
using LearnJWTAuthentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnJWTAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public AuthenticationController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost(Name="Authenticate")]
        public IActionResult Authenticate(AuthenticationRequest userCredentials){
            var result = _userServices.AuthenticateUser(userCredentials);

            if(result == null){
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}