using System.Collections.Generic;
using System.Data;
using System.Linq;
using BankApp.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BankApp.Repository
{
    public class AccountRepository 
    {
        private string connectionString;

        public  AccountRepository(IConfiguration configuration)
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
        public void Add(Accounts item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO public.accounts (money,status,accountid, email) VALUES(@Money,@Status,@AccountId,@Email)", item);
            }
        }

        public void Update(Accounts item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE public.accounts SET money = @Money , status = @Status, accountid= @AccountId, email =@Email", item);
            }
        }
        public void UpdateStatus(string id, string status)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE public.accounts SET status = @Status WHERE accountid= @Id", new{ Id = id, Status = status});
            }
        }
        public void UpdateMoney(string id, double value)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Query("UPDATE public.accounts SET money = money + @Value WHERE accountid= @Id", new{ Id = id, Value = value});
            }
        }

        public Accounts FindByAccountId(string id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Accounts>("SELECT * FROM public.accounts WHERE accountid = @Id", new { Id = id }).FirstOrDefault();
            }
        }

        public IEnumerable<Accounts> FindByEmail(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Accounts>("SELECT * FROM public.accounts WHERE email = @Email AND status = 'open'", new { Email = email });
            }
        }

        public IEnumerable<Accounts> FindALL()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Accounts>("SELECT * FROM public.accounts");
            }
        }
    }
}