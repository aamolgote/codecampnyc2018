using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Core.Interfaces
{
    public interface IBlockchainLogger
    {
        void LogException(Exception ex, string message)
    }
}
