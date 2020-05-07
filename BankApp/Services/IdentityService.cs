using System;
using System.Diagnostics.Tracing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth;
using BankApp.Repository;
using Microsoft.AspNetCore.Identity;
using BankApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BankApp
{

    public interface IIdentityService
    {
        
        Task<AuthResult> RegisterAsync(string email, string password,string name);
        Task<AuthResult> LoginAsync(string email, string password);
    }

    public class IdentityService : IIdentityService
    {
        private AuthOptions _authOptions;
        private readonly UserRepository userRepository;
        public IdentityService(IOptions<AuthOptions> authOptionsAccessor, IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
            _authOptions = authOptionsAccessor.Value;
        }
        public async Task<AuthResult> RegisterAsync(string email, string password,string name)
        {
            
            var existUser = await Task.Run(()=>userRepository.FindByEmail(email));
            if (existUser != null)
            {
                return new AuthResult
                {
                    Errors = new [] {"User with current email already exists"}
                };
            }
            var newPass = new Password(password);
            var newUser = new Users
            {
                Email = email,
                Name = name,
                Password = newPass.PasswordHash,
                Salt = newPass.Salt
            };
            await Task.Run(() => userRepository.Add(newUser));
            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email)
            };
                
            var token = new JwtSecurityToken(
                _authOptions.Issuer,
                _authOptions.Audience,
                expires: DateTime.Now.AddMinutes(_authOptions.ExpiresInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Securekey)), SecurityAlgorithms.HmacSha256Signature)
            );
            return new AuthResult
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

        }

        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            var existUser = await Task.Run(()=>userRepository.FindByEmail(email));
            if (existUser == null)
            {
                return new AuthResult
                {
                    Errors = new[] {"No such user"}
                };
            }

            if (Password.CheckPassword(password, existUser.Salt, existUser.Password))
            {
                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, existUser.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, existUser.Email)
                };
                
                var token = new JwtSecurityToken(
                    _authOptions.Issuer,
                    _authOptions.Audience,
                    expires: DateTime.Now.AddMinutes(_authOptions.ExpiresInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Securekey)), SecurityAlgorithms.HmacSha256Signature)
                );
                return new AuthResult
                {
                    Success = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            return new AuthResult
            {
                Errors = new[] {"Wrong password"}
            };
        }
        
    }
}