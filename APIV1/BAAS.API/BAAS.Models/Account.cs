using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Models
{
    public class Account
    {
        public string UserLoginId { get; set; }

        public string PassPhrase { get; set; }

        public string Address { get; set; }

        public int UserAccountId { get; set; }
    }
}
