using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class DltBlock
    {
        public ulong BlockNumber { get; set; }

        public int TransactionCount { get; set; }

        public DateTime BlockDatetime { get; set; }

        public ulong Difficulty { get; set; }

        public string BlockHash { get; set; }

        public ulong Size { get; set; }

        public ulong GasUsed { get; set; }

        public ulong GasLimit { get; set; }

        public List<DltBlockTransaction> DltTransactions { get; set; }

    }

    public class DltBlockTransaction
    {
        public string From { get; set; }

        public ulong Gas { get; set; }

        public ulong GasPrice { get; set; }

        public string Input { get; set; }

        public ulong Nonce { get; set; }

        public string To { get; set; }

        public ulong Value { get; set; }

        public string TransactionHash { get; set; }

        public int SmartContractTransactionId { get; set; }

        public int SmartContractDeployedInstanceId { get; set; }

        public string TransactionUser { get; set; }

        public string SmartContractFunction { get; set; }

        public string SmartContractFunctionParamters { get; set; }

        public List<string> SmartContractFunctionParamtersList { get; set; }

        public DateTime CreatedDatetime { get; set; }


        public DateTime UpdatedDatetime { get; set; }

        public string Name { get; set; }

        public List<SmartContractFunctionParamterInfo> SmartContractFunctionParamterNames { get; set; }



    }
}
