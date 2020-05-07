namespace Auth
{
    public class User
    {
        private Password _password;
        public User(string username, string password)
        {
            Username = username;
            _password = new Password(password);
        }
        public string Username { get; set; }

        public string Password => _password.PasswordHash;
        public string Salt => _password.Salt;
    }
}