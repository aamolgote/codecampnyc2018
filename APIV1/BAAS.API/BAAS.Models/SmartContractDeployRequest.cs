using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContractDeployRequest
    {
        public int SmartContractId { get; set; }

        public string DeployByUserLoginId { get; set; }

        public List<Object> DeploymentData { get; set; }

        public string DeployedInstanceDisplayName { get; set; }

    }

    
}
