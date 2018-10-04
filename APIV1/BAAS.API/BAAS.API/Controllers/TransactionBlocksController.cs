
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using BAAS.Core.Interfaces;
using BAAS.PlatformWrapper;

namespace BAAS.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TransactionBlocksController : ApiController
    {
        private IBlockManager blockManager;

        public TransactionBlocksController(IBlockManager blockManager)
        {
            this.blockManager = blockManager;
        }

        [Route("api/blocks/recent")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRecentBlocks(int numberOfBlocks)
        {
            try
            {
                var transactionBlocks = await this.blockManager.GetRecentBlocks(numberOfBlocks);
                return Ok(transactionBlocks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
