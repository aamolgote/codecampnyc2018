using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContractDeployedInstanceItem
    {
        public int SmartContractInstanceId { get; set; }

        public int SmartContractId { get; set; }

        public string DeployedAddress { get; set; }

        public string InitialData { get; set; }

        public string DeployByUserLoginId { get; set; }

        public string DeployedInstanceDisplayName { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public DateTime UpdatedDatetime { get; set; }

    }
}
