using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BAAS.Core.Interfaces;
using BAAS.Models;
using Newtonsoft.Json;

namespace BAAS.Db
{
    public class SmartContractDb : ISmartContractDb
    {
        public async Task<SmartContract> Create(SmartContract smartContract, List<SmartContractFunction> smartContractFunctions)
        {
            SmartContract mutatedSmartContract = null;

            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                conn.Open();
                var transaction = conn.BeginTransaction();
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.InsertSmartContract, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@name",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContract.Name
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@abi",
                    SqlDbType = System.Data.SqlDbType.Text,
                    Value = smartContract.Abi
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@byteCode",
                    SqlDbType = System.Data.SqlDbType.Text,
                    Value = smartContract.ByteCode
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@createdByUserLoginId",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContract.CreatedByUserLoginId
                });

                try
                {
                    sqlcmd.Connection = conn;
                    sqlcmd.Transaction = transaction;
                    var reader = await sqlcmd.ExecuteReaderAsync();
                    var result = reader.Read();
                    if (result)
                    {
                        mutatedSmartContract = new SmartContract();
                        mutatedSmartContract.SmartContractId = Convert.ToInt32(reader["SmartContractId"].ToString());
                        mutatedSmartContract.Name = reader["Name"]?.ToString();
                        mutatedSmartContract.Abi = reader["Abi"]?.ToString();
                        mutatedSmartContract.ByteCode = reader["ByteCode"]?.ToString();
                        mutatedSmartContract.CreatedByUserLoginId = reader["CreatedByUserLoginId"]?.ToString();
                        mutatedSmartContract.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                        mutatedSmartContract.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                    }
                    reader.Close();

                    foreach (var smartContractFunction in smartContractFunctions)
                    {
                        SqlCommand sqlCmdSmartContractFunction = new SqlCommand(StoredProcedures.InsertSmartContractFunction, conn);
                        sqlCmdSmartContractFunction.CommandType = CommandType.StoredProcedure;
                        sqlCmdSmartContractFunction.Transaction = transaction;

                        sqlCmdSmartContractFunction.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@smartContractId",
                            SqlDbType = System.Data.SqlDbType.Int,
                            Value = mutatedSmartContract.SmartContractId
                        });

                        sqlCmdSmartContractFunction.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@functionName",
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Value = smartContractFunction.FunctionName
                        });

                        sqlCmdSmartContractFunction.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@functionType",
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Value = smartContractFunction.FunctionType
                        });

                        sqlCmdSmartContractFunction.Parameters.Add(new SqlParameter()
                        {
                            ParameterName = "@sequence",
                            SqlDbType = System.Data.SqlDbType.Int,
                            Value = smartContractFunction.Sequence
                        });
                        var returnValue = await sqlCmdSmartContractFunction.ExecuteNonQueryAsync();
                    }
                    transaction.Commit();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
            return mutatedSmartContract;
        }

        public async Task<SmartContractDeployedInstanceItem> CreateSmartContractDeployedInstance(SmartContractDeployedInstanceItem smartContractDeployedInstanceItem)
        {
            SmartContractDeployedInstanceItem mutatedDeployedInstanceItem = null;
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.InsertSmartContractDeployedInstance, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractDeployedInstanceItem.SmartContractId
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@deployedAddress",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractDeployedInstanceItem.DeployedAddress
                });

                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@initialData",
                    SqlDbType = System.Data.SqlDbType.Text,
                    Value = smartContractDeployedInstanceItem.InitialData
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@deployedByUserLoginId",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractDeployedInstanceItem.DeployByUserLoginId
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@deployedInstanceDisplayName",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractDeployedInstanceItem.DeployedInstanceDisplayName
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    mutatedDeployedInstanceItem = new SmartContractDeployedInstanceItem();
                    mutatedDeployedInstanceItem.SmartContractId = Convert.ToInt32(reader["SmartContractId"]);
                    mutatedDeployedInstanceItem.SmartContractInstanceId = Convert.ToInt32(reader["SmartContractDeployedInstanceId"]);
                    mutatedDeployedInstanceItem.DeployedAddress = reader["DeployedAddress"]?.ToString();
                    mutatedDeployedInstanceItem.DeployedInstanceDisplayName = reader["DeployedInstanceDisplayName"]?.ToString();
                    mutatedDeployedInstanceItem.InitialData = reader["InitialData"]?.ToString();
                    mutatedDeployedInstanceItem.DeployByUserLoginId = reader["DeployedByUserLoginId"]?.ToString();
                    mutatedDeployedInstanceItem.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    mutatedDeployedInstanceItem.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                }
                return mutatedDeployedInstanceItem;
            }

        }

        public async Task<SmartContractTransaction> CreateSmartContractTransaction(SmartContractTransaction smartContractTransaction)
        {
            SmartContractTransaction mutatedSmartContractTransaction = null;
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.InsertSmartContractTransaction, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractDeployedInstanceId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractTransaction.SmartContractDeployedInstanceId
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@transactionHash",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractTransaction.TransactionHash
                });

                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@transactionUser",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractTransaction.TransactionUser
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractFunction",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractTransaction.SmartContractFunction
                });
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractFunctionParameters",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = smartContractTransaction.SmartContractFunctionParameters
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    mutatedSmartContractTransaction = new SmartContractTransaction();
                    mutatedSmartContractTransaction.SmartContractTransactionId = Convert.ToInt32(reader["SmartContractTransactionId"]);
                    mutatedSmartContractTransaction.SmartContractDeployedInstanceId = Convert.ToInt32(reader["SmartContractDeployedInstanceId"]);
                    mutatedSmartContractTransaction.TransactionUser = reader["TransactionUser"]?.ToString();
                    mutatedSmartContractTransaction.SmartContractFunction = reader["SmartContractFunction"]?.ToString();
                    mutatedSmartContractTransaction.SmartContractFunctionParameters = reader["SmartContractFunctionParameters"]?.ToString();
                    mutatedSmartContractTransaction.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    mutatedSmartContractTransaction.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                }
                return mutatedSmartContractTransaction;
            }
        }

        public async Task<SmartContract> GetSmartContract(int smartContractId)
        {
            SmartContract smartContract = new SmartContract();
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContract, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractId
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    smartContract.SmartContractId = Convert.ToInt32(reader["SmartContractId"]);
                    smartContract.Name = reader["Name"]?.ToString();
                    smartContract.Abi = reader["Abi"]?.ToString();
                    smartContract.ByteCode = reader["ByteCode"]?.ToString();
                    smartContract.CreatedByUserLoginId = reader["CreatedByUserLoginId"]?.ToString();
                    smartContract.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContract.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                }
            }
            smartContract.Functions = await this.GetSmartContractFunctions(smartContractId);
            return smartContract;
        }

        public async Task<List<SmartContractFunction>> GetSmartContractFunctions(int smartContractId)
        {
            List<SmartContractFunction> smartContractFunctions = new List<SmartContractFunction>();
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContractFunctions, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractId
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    SmartContractFunction smartContractFunction = new SmartContractFunction();
                    smartContractFunction.SmartContractFunctionId = Convert.ToInt32(reader["SmartContractFunctionId"]);
                    smartContractFunction.SmartContractId = Convert.ToInt32(reader["SmartContractId"]);
                    smartContractFunction.FunctionName = reader["FunctionName"]?.ToString();
                    smartContractFunction.FunctionType = reader["FunctionType"]?.ToString();
                    smartContractFunction.Sequence = Convert.ToInt32(reader["Sequence"]);
                    smartContractFunction.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContractFunction.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                    smartContractFunctions.Add(smartContractFunction);
                }
            }
            return smartContractFunctions;
        }

        public Task<SmartContract> GetSmartContract(string smartContractAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<SmartContractDeployedInstanceItem> GetSmartContractDeployedInstance(int smartContractDeployedInstanceId)
        {
            SmartContractDeployedInstanceItem smartContractDeployedInstanceItem = null;
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContractDeployedInstance, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractDeployedInstanceId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractDeployedInstanceId
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    smartContractDeployedInstanceItem = new SmartContractDeployedInstanceItem();
                    smartContractDeployedInstanceItem.SmartContractInstanceId = Convert.ToInt32(reader["SmartContractDeployedInstanceId"]);
                    smartContractDeployedInstanceItem.SmartContractId = Convert.ToInt32(reader["SmartContractId"]);
                    smartContractDeployedInstanceItem.DeployByUserLoginId = reader["DeployedByUserLoginId"]?.ToString();
                    smartContractDeployedInstanceItem.DeployedAddress = reader["DeployedAddress"]?.ToString();
                    smartContractDeployedInstanceItem.InitialData = reader["InitialData"]?.ToString();
                    smartContractDeployedInstanceItem.DeployedInstanceDisplayName = reader["DeployedInstanceDisplayName"]?.ToString();
                    smartContractDeployedInstanceItem.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContractDeployedInstanceItem.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                }
            }
            return smartContractDeployedInstanceItem;
        }

        public async Task<SmartContractDeployedInstance> GetSmartContractDeployedInstances(int smartContractId)
        {
            SmartContractDeployedInstance smartContractDeployedInstance = new SmartContractDeployedInstance();
            smartContractDeployedInstance.SmartContract = await this.GetSmartContract(smartContractId);
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContractDeployedInstances, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractId
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                smartContractDeployedInstance.SmartContractDeployedInstanceItems = new List<SmartContractDeployedInstanceItem>();
                while (reader.Read())
                {
                    SmartContractDeployedInstanceItem smartContractDeployedInstanceItem = new SmartContractDeployedInstanceItem();
                    smartContractDeployedInstanceItem.SmartContractInstanceId = Convert.ToInt32(reader["SmartContractDeployedInstanceId"]);
                    smartContractDeployedInstanceItem.SmartContractId = Convert.ToInt32(reader["SmartContractId"]);
                    smartContractDeployedInstanceItem.DeployByUserLoginId = reader["DeployedByUserLoginId"]?.ToString();
                    smartContractDeployedInstanceItem.DeployedAddress = reader["DeployedAddress"]?.ToString();
                    smartContractDeployedInstanceItem.InitialData = reader["InitialData"]?.ToString();
                    smartContractDeployedInstanceItem.DeployedInstanceDisplayName = reader["DeployedInstanceDisplayName"]?.ToString();
                    smartContractDeployedInstanceItem.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContractDeployedInstanceItem.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                    smartContractDeployedInstance.SmartContractDeployedInstanceItems.Add(smartContractDeployedInstanceItem);
                }
            }
            return smartContractDeployedInstance;
        }

        public async Task<List<SmartContract>> GetSmartContracts()
        {
            List<SmartContract> smartContracts = new List<SmartContract>();
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContracts, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    SmartContract smartContract = new SmartContract();
                    smartContract.SmartContractId = Convert.ToInt32(reader["SmartContractId"]);
                    smartContract.Name = reader["Name"]?.ToString();
                    smartContract.Abi = reader["Abi"]?.ToString();
                    smartContract.ByteCode = reader["ByteCode"]?.ToString();
                    smartContract.CreatedByUserLoginId = reader["CreatedByUserLoginId"]?.ToString();
                    smartContract.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContract.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                    smartContracts.Add(smartContract);
                }
            }
            return smartContracts;
        }

        public async Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId)
        {
            List<SmartContractTransaction> smartContractTransactions = new List<SmartContractTransaction>();
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {
                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContractTransactionsForDeployedInstance, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@smartContractDeployedInstanceId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = smartContractDeployedInstanceId
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var smartContractTransaction = new SmartContractTransaction();
                    smartContractTransaction.SmartContractTransactionId = Convert.ToInt32(reader["SmartContractTransactionId"]);
                    smartContractTransaction.SmartContractDeployedInstanceId = Convert.ToInt32(reader["SmartContractDeployedInstanceId"]);
                    smartContractTransaction.TransactionHash = reader["TransactionHash"]?.ToString();
                    smartContractTransaction.TransactionUser = reader["TransactionUser"]?.ToString();
                    smartContractTransaction.SmartContractFunction = reader["SmartContractFunction"]?.ToString();
                    smartContractTransaction.SmartContractFunctionParameters = reader["SmartContractFunctionParameters"]?.ToString();
                    smartContractTransaction.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContractTransaction.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                    smartContractTransactions.Add(smartContractTransaction);
                }
            }
            return smartContractTransactions;
        }

        public async Task<Dictionary<string, SmartContractTransaction>> GetSmartContractTransactionsInfoWithList(List<string> transactionsHashList)
        {
            string transactionHashCommaSeptList = string.Join(",", transactionsHashList.ToArray());
            using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
            {

                SqlCommand sqlcmd = new SqlCommand(StoredProcedures.GetSmartContractTransactionInfoByHash, conn);
                sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcmd.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@transactionHashCommaSepList",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Value = transactionHashCommaSeptList
                });
                conn.Open();
                var reader = await sqlcmd.ExecuteReaderAsync();
                Dictionary<string, SmartContractTransaction> smartContractTransactionDict = new Dictionary<string, SmartContractTransaction>();
                while (reader.Read())
                {
                    var smartContractTransaction = new SmartContractTransaction();
                    smartContractTransaction.SmartContractTransactionId = Convert.ToInt32(reader["SmartContractTransactionId"]);
                    smartContractTransaction.SmartContractDeployedInstanceId = Convert.ToInt32(reader["SmartContractDeployedInstanceId"]);
                    smartContractTransaction.TransactionHash = reader["TransactionHash"]?.ToString();
                    smartContractTransaction.TransactionUser = reader["TransactionUser"]?.ToString();
                    smartContractTransaction.SmartContractFunction = reader["SmartContractFunction"]?.ToString();
                    smartContractTransaction.SmartContractFunctionParameters = reader["SmartContractFunctionParameters"]?.ToString();
                    smartContractTransaction.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                    smartContractTransaction.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);

                    List<string> paramsFromDb = reader["SmartContractFunctionParameters"].ToString().Trim(new char[] { '[', ']' }).Split(',').ToList();
                    smartContractTransaction.SmartContractFunctionParametersList = paramsFromDb;

                    string abi = reader["Abi"].ToString();
                    List<string> paramNames = this.AbiToParamterNames(abi, smartContractTransaction.SmartContractFunction);

                    smartContractTransaction.SmartContractFunctionParamterNames = this.CreateParamInfo(paramNames, paramsFromDb);

                    smartContractTransactionDict[smartContractTransaction.TransactionHash] = smartContractTransaction;
                }
                return smartContractTransactionDict;
            }

        }

        private List<SmartContractFunctionParamterInfo> CreateParamInfo(List<string> names, List<string> values)
        {
            List<SmartContractFunctionParamterInfo> paramsInfo = new List<SmartContractFunctionParamterInfo>();
            int size = names.Count;
            if (size == values.Count)
            {
                for (int i = 0; i < names.Count; ++i)
                {
                    SmartContractFunctionParamterInfo param = new SmartContractFunctionParamterInfo()
                    {
                        ParamName = names[i],
                        ParamValue = values[i]
                    };
                }
            }
            return paramsInfo;
        }

        private List<string> AbiToParamterNames(string abi, string functionName)
        {
            dynamic jsonAbi = JsonConvert.DeserializeObject(abi);
            List<string> functionParameterNames = new List<string>();
            foreach (var function in jsonAbi)
            {
                if (function.name == functionName)
                {
                    foreach (var input in function.inputs)
                    {
                        string inputsName = input.name;
                        functionParameterNames.Add(inputsName);
                    }
                }
            }
            return functionParameterNames;
        }
    }
}
