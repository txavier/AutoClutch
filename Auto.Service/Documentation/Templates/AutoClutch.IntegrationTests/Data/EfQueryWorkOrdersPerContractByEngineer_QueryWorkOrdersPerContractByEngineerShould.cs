using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractTrackingManagement.DependencyResolution;

namespace ContractTrackingManagement.Infrastructure.Data.Tests
{
    [TestClass()]
    public class EfQueryWorkOrdersPerContractByEngineer_QueryWorkOrdersPerContractByEngineerShould
    {
        [TestMethod()]
        public void QueryWorkOrdersPerContractByEngineer()
        {
            // Arrange.
            var container = IoC.Initialize();

            var efQueryWorkOrdersPerContractByEngineer = container.GetInstance<Core.Interfaces.IEfQueryWorkOrdersPerContractByEngineer>();

            // Act.
            var result = efQueryWorkOrdersPerContractByEngineer.QueryWorkOrdersPerContractByEngineer(null);

            // Assert.
            Assert.IsTrue(result.Count() > 0);
        }
    }
}