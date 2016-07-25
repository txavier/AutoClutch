using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractTrackingManagement.DependencyResolution;
using ContractTrackingManagement.Core.Interfaces;

namespace ContractTrackingManagement.Core.Services.Tests
{
    [TestClass()]
    public class ContractService_QueryLowFundContractCountBySectionOfLoggedInUserShould
    {
        [TestMethod()]
        public void QueryLowFundContractCountBySectionOfLoggedInUser()
        {
            // Arrange.
            var container = IoC.Initialize();

            var contractService = container.GetInstance<IContractService>();

            // Act.
            var result = contractService.QueryLowFundContractCountBySectionOfLoggedInUser("theox", false);

            // Assert.
            Assert.IsTrue(result > 0);
        }
    }
}