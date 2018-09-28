using BAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAAS.Core.Interfaces
{
    public interface IBlockManager
    {
        Task<List<DltBlock>> GetRecentBlocks(int numberOfBlocks);

        Task<DltBlock> GetLatestBlock();
    }
}
