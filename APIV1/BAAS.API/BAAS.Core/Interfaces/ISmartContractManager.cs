using BAAS.Models;
using Newtonsoft.Json.Linq;
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

        Task<List<SmartContract>> GetSmartContracts();

        Task<SmartContract> GetSmartContract(string smartContractAddress);

        Task<SmartContractDeployedInstance> GetSmartContractDeployedInstances(int smartContractId);

        Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId);

        Task<SmartContractDeployedInstanceItem> DeploySmartContract(SmartContractDeployRequest smartContractDeployRequest);

        Task<SmartContractTransaction> ExecuteWriteFunction(SmartContractExecutionRequestPayload smartContractExecutionRequestPayload);

        Task<string> ExecuteReadFunction(SmartContractExecutionRequestPayload smartContractExecutionRequestPayload);

        Task<JToken> CompileSmartContract(SmartContractCompilePayload smartContractCompilePayload);
    }
}
