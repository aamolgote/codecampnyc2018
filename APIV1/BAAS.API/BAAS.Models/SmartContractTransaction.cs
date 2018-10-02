using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContractTransaction
    {
        public int SmartContractTransactionId { get; set; }

        public int SmartContractDeployedInstanceId { get; set; }

        public string TransactionHash { get; set; }

        public string TransactionUser { get; set; }

        public string SmartContractFunction { get; set; }

        public string SmartContractFunctionParameters { get; set; }

        public List<string> SmartContractFunctionParametersList { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

        public string Name { get; set; }

        public List<SmartContractFunctionParamterInfo> SmartContractFunctionParamterNames { get; set; }

    }
}
