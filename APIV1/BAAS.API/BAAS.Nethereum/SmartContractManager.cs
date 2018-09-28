using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;

namespace BAAS.Nethereum
{
    public class SmartContractManager : ISmartContractManager
    {
        private ISmartContractDb smartContractDb;
        private IAccountDb accountDb;
        public SmartContractManager(ISmartContractDb smartContractDb, IAccountDb accountDb)
        {

        }
        public Task<SmartContract> CreateSmartContract(SmartContract smartContract)
        {
            return null;
        }
    }
}
