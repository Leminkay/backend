using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace BankApp.Extensions
{
    public static class General
    {
        public static string GetUser(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return String.Empty;
                
            }

            return httpContext.User.Claims.Single(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
        }
    }
}