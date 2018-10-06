using Microsoft.VisualStudio.TestTools.UnitTesting;
using BAAS.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAAS.Models;

namespace BAAS.Db.Tests
{
    [TestClass()]
    public class SmartContractDbTests
    {
        [TestMethod()]
        public void CreateSmartContractDeployedInstanceTest()
        {
            SmartContractDb smartContractDb = new SmartContractDb();
            SmartContractDeployedInstanceItem deployedInstanceItem = new SmartContractDeployedInstanceItem()
            {
                DeployByUserLoginId = "aamolgote",
                InitialData = "Initial Data Test",
                DeployedAddress = "0X3838383",
                SmartContractId = 21,
                DeployedInstanceDisplayName = "Test"
            };
            var item = smartContractDb.CreateSmartContractDeployedInstance(deployedInstanceItem).Result;
            Assert.IsTrue(item != null);
        }
    }
}