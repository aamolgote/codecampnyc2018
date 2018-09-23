using BAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Core.Interfaces
{
    public interface ISmartContractManager
    {
        Task<SmartContract> CreateSmartContract(SmartContract smartContract);

    }
}
