using BAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Core.Interfaces
{
    public interface ISmartContractDb
    {
        Task<SmartContract> Create(SmartContract smartContract, List<SmartContractFunction> smartContractFunctions);

        Task<List<SmartContract>> GetSmartContracts();

        Task<SmartContract> GetSmartContract(string smartContractAddress);

        Task<SmartContract> GetSmartContract(int smartContractId);

        Task<List<SmartContractDeployedInstance>> GetSmartContractDeployedInstances(int smartContractId);

        Task<SmartContractDeployedInstanceItem> CreateSmartContractDeployedInstance(SmartContractDeployedInstanceItem smartContractDeployedInstanceItem);

        Task<SmartContractDeployedInstanceItem> GetSmartContractDeployedInstance(int smartContractDeployedInstanceItemId);

        Task<SmartContractTransaction> CreateSmartContractTransaction(SmartContractTransaction smartContractTransaction);

        Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId);

        Task<Dictionary<string, SmartContractTransaction>> GetSmartContractTransactionsInfoWithList(List<string> transactionsHashList);
    }
}
