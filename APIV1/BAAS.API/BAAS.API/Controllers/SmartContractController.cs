using BAAS.Core.Interfaces;
using BAAS.Db;
using BAAS.Logger;
using BAAS.Models;
using BAAS.Nethereum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SmartContractController : ApiController
    {
        private ISmartContractManager smartContractManager;
        private ISmartContractDb smartContractDb;
        private IAccountDb accountDb;
        private IBlockchainLogger logger;
        public SmartContractController()
        {
            this.smartContractDb = new SmartContractDb();
            this.accountDb = new AccountDb();
            this.logger = new BlockchainLogger();
            this.smartContractManager = new SmartContractManager(this.smartContractDb, this.accountDb, this.logger);
        }
        [Route("api/smartcontract/payload")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateSmartContract(SmartContract smartContractPayload)
        {
            var smartContract = await this.smartContractManager.CreateSmartContract(smartContractPayload);
            return Ok(smartContract);
        }

        [Route("api/smartcontract")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateSmartContract()
        {
            string smartContractModelData = string.Empty;
            string smartContractFileContent = string.Empty;
            SmartContract smartContractPayload = null;
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    return Ok(HttpStatusCode.UnsupportedMediaType);
                }

                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
                foreach (var stream in filesReadToProvider.Contents)
                {
                    if (stream.Headers.ContentDisposition.Name.Contains("smartContractModelData"))
                    {
                        smartContractModelData = await stream.ReadAsStringAsync();
                    }
                    else if (stream.Headers.ContentDisposition.Name.Contains("uploadFile"))
                    {
                        var fileBytes = await stream.ReadAsByteArrayAsync();
                        smartContractFileContent = System.Text.Encoding.Default.GetString(fileBytes);
                    }
                    
                }
                if (!string.IsNullOrEmpty(smartContractModelData))
                {
                    smartContractPayload = JsonConvert.DeserializeObject<SmartContract>(smartContractModelData);
                    if (!string.IsNullOrEmpty(smartContractFileContent))
                    {
                        var jsonContent = JToken.Parse(smartContractFileContent);
                        if (jsonContent != null)
                        {
                            if (jsonContent["abi"] != null)
                            {
                                smartContractPayload.Abi = jsonContent["abi"].ToString(); 
                            }

                            if (jsonContent["bytecode"] != null)
                            {
                                smartContractPayload.ByteCode = jsonContent["bytecode"].ToString();
                                if (!smartContractPayload.ByteCode.StartsWith("0x"))
                                {
                                    smartContractPayload.ByteCode = string.Concat("0x", smartContractPayload.ByteCode);
                                }
                            }
                        }
                    }
                }
                var smartContract = await this.smartContractManager.CreateSmartContract(smartContractPayload);
                return Ok(smartContract);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontracts")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSmartContracts()
        {
            try
            {
                var smartContracts = await this.smartContractManager.GetSmartContracts();
                return Ok(smartContracts);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [Route("api/smartcontract")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSmartContract(string smartContractAddress)
        {
            try
            {
                var smartContract = await this.smartContractManager.GetSmartContract(smartContractAddress);
                return Ok(smartContract);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontract/instances")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSmartContractDeployedInstances(int smartContractId)
        {
            try
            {
                var smartContractDeployedInstance = await this.smartContractManager.GetSmartContractDeployedInstances(smartContractId);
                return Ok(smartContractDeployedInstance);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontract/instance/transactions")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSmartContractTransactionsForDeployedInstance(int smartContractDeployedInstanceId)
        {
            try
            {
                var smartContractTransactions = await this.smartContractManager.GetSmartContractTransactionsForDeployedInstance(smartContractDeployedInstanceId);
                return Ok(smartContractTransactions);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontract/deploy")]
        [HttpPost]
        public async Task<IHttpActionResult> DeploySmartContract([FromBody]SmartContractDeployRequest smartContractDeployRequest)
        {
            try
            {
                var smartContractDeployedInstanceItem = await this.smartContractManager.DeploySmartContract(smartContractDeployRequest);
                return Ok(smartContractDeployedInstanceItem);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontract/instance/executewritefunction")]
        [HttpPost]
        public async Task<IHttpActionResult> ExecuteWriteFunction([FromBody]SmartContractExecutionRequestPayload smartContractExecutionRequestPayload)
        {
            try
            {
                var smartContractDeployedInstanceItem = await this.smartContractManager.ExecuteWriteFunction(smartContractExecutionRequestPayload);
                return Ok(smartContractDeployedInstanceItem);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontract/instance/executereadfunction")]
        [HttpPost]
        public async Task<IHttpActionResult> ExecuteReadFunction([FromBody]SmartContractExecutionRequestPayload smartContractExecutionRequestPayload)
        {
            try
            {
                var smartContractDeployedInstanceItem = await this.smartContractManager.ExecuteReadFunction(smartContractExecutionRequestPayload);
                return Ok(smartContractDeployedInstanceItem);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/smartcontract/compile")]
        [HttpPost]
        public async Task<IHttpActionResult> CompileSmartContract([FromBody]SmartContractCompilePayload smartContractCompilePayload)
        {
            try
            {
                JToken compileReponse = await this.smartContractManager.CompileSmartContract(smartContractCompilePayload);
                return Ok(compileReponse);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
