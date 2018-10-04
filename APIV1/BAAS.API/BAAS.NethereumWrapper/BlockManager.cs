using BAAS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;

namespace BAAS.NethereumWrapper
{
    public class BlockManager : IBlockManager
    {
        public Task<DltBlock> GetLatestBlock()
        {
            throw new NotImplementedException();
        }

        public async Task<List<DltBlock>> GetRecentBlocks(int numberOfBlocks)
        {
            List<DltBlock> dltBlocks = new List<DltBlock>();
            return dltBlocks;
        }
    }
}
