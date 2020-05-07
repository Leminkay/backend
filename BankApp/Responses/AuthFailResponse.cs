using System.Collections;
using System.Collections.Generic;

namespace BankApp.Responses
{
    public class AuthFailResponse
    {
        public IEnumerable <string> Errors { get; set; }
    }
}