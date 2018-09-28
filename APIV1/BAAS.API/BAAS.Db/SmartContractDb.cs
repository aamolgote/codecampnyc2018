using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BAAS.Core.Interfaces;
using BAAS.Models;

namespace BAAS.Db
{
    public class SmartContractDb : ISmartContractDb
    {
        public async Task<SmartContract> Create(SmartContract smartContract, List<SmartContractFunction> smartContractFunctions)
        {
            SmartContract mutatedSmartContract = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(DbConfiguration.ConnectionString))
                {
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
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        sqlcmd.Connection = conn;
                        using (var reader = await sqlcmd.ExecuteReaderAsync())
                        {
                            var result = reader.Read();
                            if (result)
                            {
                                mutatedSmartContract.SmartContractId = Convert.ToInt32(reader["SmartContractId"].ToString());
                                mutatedSmartContract.Name = reader["Name"]?.ToString();
                                mutatedSmartContract.Abi = reader["Abi"]?.ToString();
                                mutatedSmartContract.ByteCode = reader["ByteCode"]?.ToString();
                                mutatedSmartContract.CreatedByUserLoginId = reader["CreatedByUserLoginId"]?.ToString();
                                mutatedSmartContract.CreatedDatetime = string.IsNullOrEmpty(reader["CreatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["CreatedDatetime"]);
                                mutatedSmartContract.UpdatedDatetime = string.IsNullOrEmpty(reader["UpdatedDatetime"]?.ToString()) ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedDatetime"]);
                            }
                        }

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
                                ParameterName = "@funtionName",
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
                            var returnValue = await sqlCmdSmartContractFunction.ExecuteReaderAsync();
                        }
                        transaction.Commit();
                        conn.Close();
                    }
                }
                return mutatedSmartContract;
            }
            catch(Exception ex)
            {
                
            }
            
        }

        public Task<SmartContractDeployedInstanceItem> CreateSmartContractDeployedInstance(SmartContractDeployedInstanceItem smartContractDeployedInstanceItem)
        {
            throw new NotImplementedException();
        }

        public Task<SmartContractTransaction> CreateSmartContractTransaction(SmartContractTransaction smartContractTransaction)
        {
            throw new NotImplementedException();
        }
    ===================
        public Task<SmartContract> GetSmartContract(int smartContractId)
        {
            throw new NotImplementedException();
        }

        public Task<SmartContract> GetSmartContract(string smartContractAddress)
        {
            throw new NotImplementedException();
        }

        public Task<SmartContractDeployedInstanceItem> GetSmartContractDeployedInstance(int smartContractDeployedInstanceItemId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SmartContractDeployedInstance>> GetSmartContractDeployedInstances(int smartContractId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SmartContract>> GetSmartContracts()
        {
            throw new NotImplementedException();
        }

        public Task<List<SmartContractTransaction>> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, SmartContractTransaction>> GetSmartContractTransactionsInfoWithList(List<string> transactionsHashList)
        {
            throw new NotImplementedException();
        }
    }
}
