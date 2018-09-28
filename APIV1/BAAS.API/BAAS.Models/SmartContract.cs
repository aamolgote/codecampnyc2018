using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContract
    {
        public int SmartContractId { get; set; }
        public string Abi { get; set; }

        public string ByteCode { get; set; }

        public string Name { get; set; }

        public string CreatedByUserLoginId { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

        public List<SmartContractFunction> Functions { get; set; }
    }

   
}
