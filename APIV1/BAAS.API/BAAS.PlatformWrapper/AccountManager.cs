using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;

namespace BAAS.PlatformWrapper
{
    public class AccountManager : IAccountManager
    {
        private IAccountDb accountDb;
        public AccountManager(IAccountDb accDb)
        {
            this.accountDb = accDb;
        }
        public Task<string> Create(Account accountPayload)
        {
            throw new NotImplementedException();
        }

        public Task<Account> GetAccount(string accountAddress)
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetAccounts()
        {
            return this.accountDb.GetAccounts();
        }
    }
}
