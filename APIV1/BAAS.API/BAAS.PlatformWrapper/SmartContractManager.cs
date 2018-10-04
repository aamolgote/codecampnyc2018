using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nethereum.Geth;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.Compilation;
using Nethereum.Web3;


namespace BAAS.PlatformWrapper
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
                    foreach (var smartContractAbiFunction in smartContractAbiFunctions)
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
            catch (Exception ex)
            {
                this.logger.LogException(ex, $"Error occured in {typeof(SmartContractManager)}.GetSmartContracts");
                throw;
            }
        }

        public async Task<SmartContract> GetSmartContract(string smartContractAddress)
        {
            try
            {
                var smartContract = await this.smartContractDb.GetSmartContract(smartContractAddress);
                return smartContract;
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, $"Error occured in {typeof(SmartContractManager)}.GetSmartContract");
                throw;
            }
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

        public async Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId)
        {
            try
            {
                var smartContractDeployedInstance = await this.smartContractDb.GetSmartContractTransactionsForDeployedInstance(smartContractDeployedInstanceId);
                return smartContractDeployedInstance;
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, $"Error occured in {typeof(SmartContractManager)}.GetSmartContractTransactionsForDeployedInstance");
                throw;
            }
        }

        public async Task<SmartContractDeployedInstanceItem> DeploySmartContract(SmartContractDeployRequest smartContractDeployRequest)
        {
            try
            {
                var smartContract = await this.smartContractDb.GetSmartContract(smartContractDeployRequest.SmartContractId);
                var userDltAccount = await this.accountDb.GetUserDltAccountByLoginId(smartContractDeployRequest.DeployByUserLoginId);
                if (smartContract != null)
                {
                    var web3 = new Web3Geth();
                    var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(userDltAccount.Address, userDltAccount.PassPhrase, 120);

                    object[] parameters = null;
                    if (smartContractDeployRequest.DeploymentData != null && smartContractDeployRequest.DeploymentData.Count > 0)
                    {
                        parameters = smartContractDeployRequest.DeploymentData.ToArray();
                    }

                    if (unlockResult)
                    {
                        var gas = await web3.Eth.DeployContract.EstimateGasAsync(smartContract.Abi,
                                                                                    smartContract.ByteCode,
                                                                                    userDltAccount.Address,
                                                                                    parameters);

                        var receipt = await web3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(smartContract.Abi,
                                                                                                      smartContract.ByteCode,
                                                                                                      userDltAccount.Address,
                                                                                                      gas, null, parameters);

                        string contractAddress = receipt.ContractAddress;

                        SmartContractDeployedInstanceItem deployedInstanceItem = new SmartContractDeployedInstanceItem()
                        {
                            DeployByUserLoginId = smartContractDeployRequest.DeployByUserLoginId,
                            InitialData = Newtonsoft.Json.JsonConvert.SerializeObject(smartContractDeployRequest.DeploymentData),
                            DeployedAddress = contractAddress,
                            SmartContractId = smartContractDeployRequest.SmartContractId,
                            DeployedInstanceDisplayName = smartContractDeployRequest.DeployedInstanceDisplayName
                        };

                        var mutatedSmartContractDeployedInstanceItem = await this.smartContractDb.CreateSmartContractDeployedInstance(deployedInstanceItem);
                        return mutatedSmartContractDeployedInstanceItem;
                    }
                    else
                    {
                        throw new Exception("Unable to unlock account");
                    }
                }
                else
                {
                    throw new Exception("Invalid Smart Contract Id");
                }

            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, "An error occured in method SmartContractManager ==> DeploySmartContract");
                throw;
            }
        }

        public async Task<SmartContractTransaction> ExecuteWriteFunction(SmartContractExecutionRequestPayload smartContractExecutionRequestPayload)
        {
            if (smartContractExecutionRequestPayload == null)
                throw new ArgumentNullException(nameof(smartContractExecutionRequestPayload) + " is null");

            if (smartContractExecutionRequestPayload.SmartContractDeployedInstanceId == 0)
                throw new ArgumentException(nameof(smartContractExecutionRequestPayload.SmartContractDeployedInstanceId) + " is not valid");

            if (string.IsNullOrEmpty(smartContractExecutionRequestPayload.TransactionUser))
                throw new ArgumentException(nameof(smartContractExecutionRequestPayload.TransactionUser) + " is not valid");

            try
            {
                var userDltAccount = await this.accountDb.GetUserDltAccountByLoginId(smartContractExecutionRequestPayload.TransactionUser);
                var smartContractDeployedInstance = await this.smartContractDb.GetSmartContractDeployedInstance(smartContractExecutionRequestPayload.SmartContractDeployedInstanceId);
                if (userDltAccount != null && smartContractDeployedInstance != null)
                {
                    var smartContract = await this.smartContractDb.GetSmartContract(smartContractDeployedInstance.SmartContractId);
                    if (smartContract != null)
                    {
                        var web3 = new Web3Geth();
                        var unlockResult = await web3.Personal.UnlockAccount.SendRequestAsync(userDltAccount.Address, userDltAccount.PassPhrase, 120);
                        if (unlockResult)
                        {
                            var contract = web3.Eth.GetContract(smartContract.Abi, smartContractDeployedInstance.DeployedAddress);
                            var contractFunction = contract.GetFunction(smartContractExecutionRequestPayload.Function);
                            var param = smartContractExecutionRequestPayload.Parameters;
                            var functionDataParam = param.ToArray();
                            var gas = await contractFunction.EstimateGasAsync(userDltAccount.Address, new HexBigInteger(9000), null, functionDataParam);
                            string transactionHash = await contractFunction.SendTransactionAsync(userDltAccount.Address, gas, null, functionDataParam);
                            if (!string.IsNullOrEmpty(transactionHash))
                            {
                                SmartContractTransaction smartContractTransaction = new SmartContractTransaction()
                                {
                                    SmartContractDeployedInstanceId = smartContractExecutionRequestPayload.SmartContractDeployedInstanceId,
                                    SmartContractFunction = smartContractExecutionRequestPayload.Function,
                                    TransactionHash = transactionHash,
                                    TransactionUser = smartContractExecutionRequestPayload.TransactionUser,
                                    SmartContractFunctionParameters = Newtonsoft.Json.JsonConvert.SerializeObject(smartContractExecutionRequestPayload.Parameters)
                                };
                                var updatedSmartContractTransaction = await this.smartContractDb.CreateSmartContractTransaction(smartContractTransaction);
                                return updatedSmartContractTransaction;
                            }
                            else
                            {
                                throw new Exception("Generate transaction hash is null");
                            }
                        }
                        else
                        {
                            throw new Exception("Unable to unlock the transaction user DLT account for performing the DLT transaction");
                        }

                    }
                    else
                    {
                        throw new Exception("Unabel to unlock the account");
                    }

                }
                else
                {
                    throw new Exception("Unable to find Smart Contract");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, "An error occured in method SmartContractManager ==> ExecuteWriteFunction");
                throw;
            }
        }

        public async Task<string> ExecuteReadFunction(SmartContractExecutionRequestPayload smartContractExecutionRequestPayload)
        {
            try
            {
                if (smartContractExecutionRequestPayload == null)
                    throw new ArgumentNullException(nameof(smartContractExecutionRequestPayload) + " is null");

                if (smartContractExecutionRequestPayload.SmartContractDeployedInstanceId == 0)
                    throw new ArgumentException(nameof(smartContractExecutionRequestPayload.SmartContractDeployedInstanceId) + " is not valid");

                var smartContractDeployedInstance = await this.smartContractDb.GetSmartContractDeployedInstance(smartContractExecutionRequestPayload.SmartContractDeployedInstanceId);

                if (smartContractDeployedInstance != null)
                {
                    var smartContract = await this.smartContractDb.GetSmartContract(smartContractDeployedInstance.SmartContractId);
                    if (smartContract != null)
                    {
                        var web3 = new Web3Geth();
                        var contract = web3.Eth.GetContract(smartContract.Abi, smartContractDeployedInstance.DeployedAddress);
                        var contractFunction = contract.GetFunction(smartContractExecutionRequestPayload.Function);
                        var param = smartContractExecutionRequestPayload.Parameters;
                        Object functionResult = null;
                        if (param.Count > 0)
                        {
                            functionResult = await contractFunction.CallAsync<Object>(param.ToArray());
                        }
                        else
                        {
                            functionResult = await contractFunction.CallAsync<Object>();
                        }

                        return functionResult.ToString();
                    }
                    else
                    {
                        throw new Exception("Unable to find Smart Contract");
                    }
                }
                else
                {
                    throw new Exception("Unable to find Smart Contract deployed instance");
                }
            }
            catch(Exception ex)
            {
                this.logger.LogException(ex, "An error occured in method SmartContractManager ==> ExecuteReadFunction");
                throw;
            }
        }

        public Task<JToken> CompileSmartContract(SmartContractCompilePayload smartContractCompilePayload)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex, "An error occured in method SmartContractManager ==> CompileSmartContract");
                throw;
            }
        }
    }
}
