using BAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Core.Interfaces
{
    public interface IAccountManager
    {
        Task<string> Create(Account accountPayload);

        Task<Account> GetAccount(string accountAddress);

        Task<List<Account>> GetAccounts();
    }
}
