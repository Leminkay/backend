namespace BankApp
{
    public class AuthOptions
    {
        public string Securekey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}