using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContractDeployedInstance
    {
        public SmartContract SmartContract { get; set; }

        public List<SmartContractDeployedInstanceItem> SmartContractDeployedInstanceItems { get; set; }

    }
}
