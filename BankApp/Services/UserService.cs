
namespace Auth
{
    public interface IUserService
    {
        bool IsValidUser(string username, string password, string salt);
    }
    public class UserService : IUserService
    {

        public bool IsValidUser(string username, string password,string salt)
        {
            
            return Password.CheckPassword(password, salt, password);
        }
    }
}