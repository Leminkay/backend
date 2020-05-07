using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth;
using BankApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BankApp.Controllers
{
    
    [Route("api/[controller]")]
    public class TokenController
    {
        private IUserService _service;
        private AuthOptions _authOptions;
        public TokenController(IUserService service, IOptions<AuthOptions> authOptionsAccessor)
        {
            _service = service;
            _authOptions = authOptionsAccessor.Value;
        }
        
        [HttpPost]
        public IActionResult Get([FromBody] UserCredentials user)
        {
            
            if (_service.IsValidUser(user.Username, user.Password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                
                var token = new JwtSecurityToken(
                    _authOptions.Issuer,
                    _authOptions.Audience,
                    expires: DateTime.Now.AddMinutes(_authOptions.ExpiresInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Securekey)), SecurityAlgorithms.HmacSha256Signature)
                    );
                return new OkObjectResult(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return new UnauthorizedResult();
            

        }
        
    }
}