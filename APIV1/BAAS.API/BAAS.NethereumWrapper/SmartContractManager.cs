using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BAAS.NethereumWrapper
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
        public async Task<SmartContract> CreateSmartContract(SmartContract smartContract)
        {
            try
            {
                var smartContractAbiFunctions = JsonConvert.DeserializeObject<List<SmartContractAbi>>(smartContract.Abi);
                List<SmartContractFunction> smartContractFunctions = null;
                if (smartContractAbiFunctions != null && smartContractAbiFunctions.Count > 0)
                {
                    smartContractFunctions = new List<SmartContractFunction>();
                    int sequence = 1;
                    foreach(var smartContractAbiFunction in smartContractAbiFunctions)
                    {
                        if (smartContractAbiFunction.Type.ToLower() != "constructor" 
                            && smartContractAbiFunction.Type.ToLower() != "event")
                        {
                            SmartContractFunction smartContractFunction = new SmartContractFunction()
                            {
                                FunctionName = smartContractAbiFunction.Name,
                                FunctionType = smartContractAbiFunction.Type,
                                Sequence = sequence++
                            };
                            smartContractFunctions.Add(smartContractFunction);
                        }
                    }
                }
                var smartContractUpdated = await this.smartContractDb.Create(smartContract, smartContractFunctions);
                return smartContractUpdated;
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, $"Error occured in {typeof(SmartContractManager)}.CreateSmartContract");
                throw;
            }
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

        public async Task<SmartContractDeployedInstance> GetSmartContractDeployedInstances(int smartContractId)
        {
            try
            {
                var smartContractDeployedInstance = await this.smartContractDb.GetSmartContractDeployedInstances(smartContractId);
                return smartContractDeployedInstance;
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, $"Error occured in {typeof(SmartContractManager)}.GetSmartContracts");
                throw;
            }
        }

        public Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId)
        {
            throw new NotImplementedException();
        }

        public async Task<SmartContractDeployedInstanceItem> DeploySmartContract(SmartContractDeployRequest smartContractDeployRequest)
        {
            try
            {
                var smartContract = await this.smartContractDb.GetSmartContract(smartContractDeployRequest.SmartContractId);
                var userDltAccount = await this.accountDb.GetUserDltAccountByLoginId(smartContractDeployRequest.DeployByUserLoginId);
                if (smartContract != null)
                {

                    var web3 = Nethereum.Geth.Web3Geth("http://localhost:8545/");
                    //var unlockResult = await 
                    
                }
                
            }
            catch(Exception ex)
            {

            }
            return null;
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
