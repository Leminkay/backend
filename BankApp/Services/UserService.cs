
using BankApp.Repository;
using Microsoft.Extensions.Configuration;

namespace Auth
{
    public interface IUserService
    {
        bool IsValidUser(string email, string password);
    }
    public class UserService : IUserService
    {
        private readonly UserRepository userRepository;

        public UserService(IConfiguration configuration)
        {
            userRepository = new UserRepository(configuration);
        }

        public bool IsValidUser(string email, string password)
        {
            var account = userRepository.FindByEmail(email);
            if (account == null)
            {
                return false;
            }
            
            return Password.CheckPassword(password, account.Salt, account.Password);
        }
    }
}