using System.Threading.Tasks;
using Auth;
using BankApp.DTO;
using BankApp.Repository;
using BankApp.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    
    public class IdentityController
    {
        
        private readonly IIdentityService _identityService;
        public IdentityController (IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [Route("api/[controller]/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegistrationCredentials request)
        {
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password, request.Name);
            if (!authResponse.Success)
            {
                return new BadRequestObjectResult(new AuthFailResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return new OkObjectResult(new
            {
                token = authResponse.Token
            });
        }
        [Route("api/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCredentials request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return new BadRequestObjectResult(new AuthFailResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return new OkObjectResult(new
            {
                token = authResponse.Token
            });
        }
    }
}