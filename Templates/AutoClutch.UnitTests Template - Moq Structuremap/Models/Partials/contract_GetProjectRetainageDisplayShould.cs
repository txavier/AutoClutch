using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractTrackingManagement.Core.Models
{
    [TestClass()]
    public class contract_GetProjectRetainageDisplayShould
    {
        [TestMethod()]
        public void GetProjectRetainageDisplay()
        {
            // Arrange.
            var contract = new contract() { projectRetainage = 0.1 };

            // Act.
            var result = contract.GetProjectRetainageDisplay();

            // Assert.
            Assert.AreEqual(10, result);
        }
    }
}