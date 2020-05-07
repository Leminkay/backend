using System.Collections.Generic;
using System.Data;
using System.Linq;
using BankApp.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BankApp.Repository
{
    public class UserRepository 
    {
        private string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }
        public void Add(Users item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO public.users (name,email,salt, password) VALUES(@Name,@Email,@Salt,@Password)", item);
            }
        }

        public void Remove(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM public.users WHERE Id=@Id", new { Id = id });
            }
        }

        public void Update(Users item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE public.users SET name = @Name, email= @Email, salt= @Salt, password =@password WHERE id = @Id", item);
            }
        }

        public Users FindByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Users>("SELECT * FROM public.users WHERE id = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public Users FindByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Users>("SELECT * FROM public.users WHERE email = @Email", new { Email = email }).FirstOrDefault();
            }
        }

        public IEnumerable<Users> FindALL()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Users>("SELECT * FROM public.users");
            }
        }
    }
}