using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;
using System.Data.SqlClient;

namespace BAAS.Db
{
    public class AccountDb : IAccountDb
    {
        public async Task<List<Account>> GetAccounts()
        {
            List<Account> accounts = new List<Account>();
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetUserDltAccounts, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                using (var reader = await sqlcmd.ExecuteReaderAsync())
                {

                    while (reader.Read())
                    {
                        Account account = new Account();
                        account.UserLoginId = reader["UserLoginId"]?.ToString();
                        account.PassPhrase = reader["Passphrase"]?.ToString();
                        account.Address = reader["AccountAddress"]?.ToString();
                        account.UserAccountId = Convert.ToInt32(reader["UserAccountId"].ToString());
                        accounts.Add(account);
                    }
                }
            }
            return accounts;
        }

        public async Task<Account> GetUserDltAccountByLoginId(string userLoginId)
        {
            Account account = null;
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetUserDltAccountByLoginId, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@userLoginId",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = userLoginId
                });
                conn.Open();
                using (var reader = await sqlcmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        account = new Account();
                        account.UserAccountId = Convert.ToInt32(reader["UserAccountId"].ToString());
                        account.UserLoginId = reader["UserLoginId"]?.ToString();
                        account.PassPhrase = reader["Passphrase"]?.ToString();
                        account.Address = reader["AccountAddress"]?.ToString();

                    }
                }
            }
            return account;
        }
    }
}
