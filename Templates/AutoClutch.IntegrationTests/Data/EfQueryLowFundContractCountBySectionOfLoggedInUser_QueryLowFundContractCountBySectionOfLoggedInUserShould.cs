using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractTrackingManagement.DependencyResolution;
using ContractTrackingManagement.Core.Interfaces;

namespace ContractTrackingManagement.Infrastructure.Data.Tests
{
    [TestClass()]
    public class EfQueryLowFundContractCountBySectionOfLoggedInUser_QueryLowFundContractCountBySectionOfLoggedInUserShould
    {
        [TestMethod()]
        public void QueryLowFundContractCountBySectionOfLoggedInUser()
        {
            // Arrange.
            var container = IoC.Initialize();

            var efQueryLowFundContractCountBySectionOfLoggedInUser = container.GetInstance<IEfQueryLowFundContractCountBySectionOfLoggedInUser>();

            // Act.
            var result = efQueryLowFundContractCountBySectionOfLoggedInUser.QueryLowFundContractCountBySectionOfLoggedInUser("theox", false);

            // Assert.
            Assert.IsTrue(result > 0);
        }
    }
}