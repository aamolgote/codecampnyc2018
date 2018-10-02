using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Db
{
    public class StoredProcedures
    {
        public const string GetUserDltAccounts = "GetUserDltAccounts";
        public const string GetUserDltAccountByLoginId = "GetUserDltAccountByLoginId";
        public const string InsertSmartContract = "InsertSmartContract";
        public const string InsertSmartContractFunction = "InsertSmartContractFunction";
        public const string GetSmartContracts = "GetSmartContracts";
        public const string GetSmartContractDeployedInstances = "GetSmartContractDeployedInstances";
        public const string GetSmartContract = "GetSmartContract";
        public const string GetSmartContractFunctions = "GetSmartContractFunctions";

        public const string InsertSmartContractDeployedInstance = "InsertSmartContractDeployedInstance";
        public const string GetSmartContractDeployedInstance = "GetSmartContractDeployedInstance";
        public const string InsertSmartContractTransaction = "InsertSmartContractTransaction";
        public const string GetSmartContractTransactionsForDeployedInstance = "GetSmartContractTransactionsForDeployedInstance";
        //public const string GetSmartContracts = "GetSmartContracts";
        //public const string GetSmartContractDeployedInstances = "GetSmartContractDeployedInstances";
        //public const string GetSmartContract = "GetSmartContract";

    }
}
