using System;
using System.Linq;
using System.Web.Http.ModelBinding;
using BankApp.DTO;
using BankApp.Extensions;
using BankApp.Models;
using BankApp.Repository;


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BankApp.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class AccountController: Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AccountRepository accountRepository;
        public AccountController( IConfiguration configuration)
        {
            accountRepository = new AccountRepository(configuration);
            
        }
        private static Random random = new Random();
        private static int length = 9;
        public static string RandomString()
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        

        [Route("api/[controller]/newaccount")]
        [HttpPost]
        public IActionResult NewAccount([FromBody] NewAccCredentials request)
        {
            if (!ModelState.IsValid)
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            if (HttpContext.GetUser() != request.Email)
            {
                return new BadRequestObjectResult(new
                {
                    Errors =  "No access"
                });
            }

            string accountid ;
            do
            {
                accountid = 4 + RandomString();
            } while (accountRepository.FindByAccountId(accountid) != null);

            var newAcc = new Accounts
            {
                AccountId = accountid,
                Email = request.Email,
                Money = 0,
                Status = "open"
            };
            
            accountRepository.Add(newAcc);
            
            return new OkObjectResult(new
            {
                AccountId = accountid
            });
        }
        [Route("api/[controller]/status")]
        [HttpPost]
        public IActionResult Status([FromBody] AccountStatusCredentials request)
        {
            if (!ModelState.IsValid)
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            if (!(request.Status == "open" || request.Status == "closed"))
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  "not open/closed status"
                });
            }
            
            
            var account = accountRepository.FindByAccountId(request.Id);
            if (HttpContext.GetUser() != account.Email)
            {
                return new BadRequestObjectResult(new
                {
                    Errors =  "No access"
                });
            }
            accountRepository.UpdateStatus(request.Id, request.Status);
   
            return new OkObjectResult(new
            {
                Status = request.Status
            });
        }
        [Route("api/[controller]/transfer")]
        [HttpPost]
        public IActionResult Transfer([FromBody] TransferCredentials request)
        {
            if (!ModelState.IsValid)
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            if (request.Value <= 0)
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  "not valid value"
                });
            }
            
            
            var accountFrom = accountRepository.FindByAccountId(request.FromId);
            var accountTo = accountRepository.FindByAccountId(request.ToId);
            if (accountFrom == null || accountTo == null || accountFrom.Status == "closed" || accountTo.Status == "closed")
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  "not valid Account Id"
                });
            }
            
            if (HttpContext.GetUser() != accountFrom.Email)
            {
                return new BadRequestObjectResult(new
                {
                    Errors =  "No access"
                });
            }

            if (accountFrom.Money < request.Value)
            {
                return new BadRequestObjectResult(new
                {
                    Errors =  "Invalid balance"
                });
            }
            accountRepository.UpdateMoney(accountFrom.AccountId, -request.Value);
            accountRepository.UpdateMoney(accountTo.AccountId, request.Value);
   
            return new OkObjectResult(new
            {
                Message = "success"
            });
        }
        [Route("api/[controller]/deposit")]
        [HttpPost]
        public IActionResult Deposit([FromBody] DepositCredentials request)
        {
            if (!ModelState.IsValid)
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            if (request.Value <= 0)
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  "not valid value"
                });
            }
            
            
            var accountFrom = accountRepository.FindByAccountId(request.Id);
            if (accountFrom == null || accountFrom.Status == "closed")
            {
                return  new BadRequestObjectResult(new
                {
                    Errors =  "not valid Account Id"
                });
            }
            
            if (HttpContext.GetUser() != accountFrom.Email)
            {
                return new BadRequestObjectResult(new
                {
                    Errors =  "No access"
                });
            }

            
            accountRepository.UpdateMoney(accountFrom.AccountId, request.Value);
            
   
            return new OkObjectResult(new
            {
                Message = "success"
            });
        }
    }
}