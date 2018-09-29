using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;
using Newtonsoft.Json.Linq;

namespace BAAS.Nethereum
{
    public class SmartContractManager : ISmartContractManager
    {
        private ISmartContractDb smartContractDb;
        private IAccountDb accountDb;
        private IBlockchainLogger logger;
        public SmartContractManager(ISmartContractDb smartContractDb, IAccountDb accountDb, IBlockchainLogger logger)
        {
            this.smartContractDb = smartContractDb;
            this.accountDb = accountDb;
            this.logger = logger;
        }
        public Task<SmartContract> CreateSmartContract(SmartContract smartContract)
        {
            return null;
        }

        public async Task<List<SmartContract>> GetSmartContracts()
        {
            try
            {
                var smartContracts = await this.smartContractDb.GetSmartContracts();
                return smartContracts;
            }
            catch(Exception ex)
            {
                this.logger.LogException(ex, $"Error occured in {typeof(SmartContractManager)}.GetSmartContracts");
                throw;
            }
        }

        public Task<SmartContract> GetSmartContract(string smartContractAddress)
        {
            throw new NotImplementedException();
        }

        public Task<List<SmartContractDeployedInstance>> GetSmartContractDeployedInstances(int smartContractId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<SmartContractDeployedInstanceItem> DeploySmartContract(SmartContractDeployRequest smartContractDeployRequest)
        {
            throw new NotImplementedException();
        }

        public Task<SmartContractDeployedInstanceItem> ExecuteWriteFunction(SmartContractExecutionRequestPayload smartContractExecutionRequestPayload)
        {
            throw new NotImplementedException();
        }

        public Task<SmartContractDeployedInstanceItem> ExecuteReadFunction(SmartContractExecutionRequestPayload smartContractExecutionRequestPayload)
        {
            throw new NotImplementedException();
        }

        public Task<JToken> CompileSmartContract(SmartContractCompilePayload smartContractCompilePayload)
        {
            throw new NotImplementedException();
        }
    }
}
