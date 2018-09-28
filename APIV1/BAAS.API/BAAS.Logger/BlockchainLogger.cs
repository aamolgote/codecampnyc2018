using BAAS.Core.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Logger
{
    public class BlockchainLogger : IBlockchainLogger
    {
        public void LogException(Exception ex, string message)
        {
            NLog.Logger logger = LogManager.GetCurrentClassLogger();
            logger.Error(ex, message);
        }
    }
}
