using BAAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BAAS.API.Controllers
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class SmartContractController : ApiController
    {
        [Route("api/smartcontract")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateSmartContract(SmartContract smartContract)
        {
            return Ok();
        }


    }
}
