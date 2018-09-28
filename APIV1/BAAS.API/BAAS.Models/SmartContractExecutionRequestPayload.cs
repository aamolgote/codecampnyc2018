using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class SmartContractExecutionRequestPayload
    {
        public int SmartContractDeployedInstanceId { get; set; }

        public string Function { get; set; }

        public List<Object> Parameters { get; set; }

        public string TransactionUser { get; set; }

    }
}
