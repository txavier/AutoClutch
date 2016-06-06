using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractTrackingManagement.Core.Models.Tests
{
    [TestClass()]
    public class contract_GetCurrentContractValueDisplayShould
    {
        [TestMethod()]
        public void GetCurrentContractValueDisplay()
        {
            // Arrange.
            var contract = new contract()
            {
                originalContractValue = 12,
                changeOrders = new List<changeOrder>()
                {
                    new changeOrder { registeredAmount = 10 },
                    new changeOrder { registeredAmount = 10 }
                }
            };

            // Act.
            var result = contract.GetCurrentContractValueDisplay(contract.originalContractValue, contract.changeOrders);

            // Assert.
            Assert.AreEqual(32, result);
        }
    }
}