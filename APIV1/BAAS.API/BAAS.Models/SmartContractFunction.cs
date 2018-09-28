using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContractFunction
    {
        public int SmartContractFunctionId { get; set; }

        public int SmartContractId { get; set; }

        public string FunctionName { get; set; }

        public string FunctionType { get; set; }

        public int Sequence { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

    }

    public class SmartContractFunctionParamterInfo
    {
        public string ParamName { get; set; }

        public string ParamValue { get; set; }

    }


    public class SmartContractAbi
    {
        public bool Constant { get; set; }

        public List<object> Inputs { get; set; }

        public string Name { get; set; }

        public List<object> Outputs { get; set; }

        public bool Payable { get; set; }

        public string StateMutability { get; set; }

        public string Type { get; set; }

        public bool Anonymous { get; set; }

    }
}
