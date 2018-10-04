using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using BAAS.PlatformWrapper;
using BAAS.Core.Interfaces;
using BAAS.Db;

namespace BAAS.API.Controllers
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class AccountController : ApiController
    {
        private IAccountManager accountManager;
        private IAccountDb accountDb;
        
        
        public AccountController()
        {
            this.accountDb = new AccountDb();
            this.accountManager = new AccountManager(this.accountDb);
        }

        [Route("api/useraccounts")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAccounts()
        {
            try
            {
                var userAccounts = await this.accountManager.GetAccounts();
                return Ok(userAccounts);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
