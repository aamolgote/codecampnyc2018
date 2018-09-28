using BAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Core.Interfaces
{
    public interface IAccountDb
    {
        Task<List<Account>> GetAccounts();

        Task<Account> GetUserDltAccountByLoginId(string userLoginId);
    }
}
