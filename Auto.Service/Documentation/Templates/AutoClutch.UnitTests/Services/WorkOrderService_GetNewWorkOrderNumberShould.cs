using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap.AutoMocking.Moq;
using Moq;
using ContractTrackingManagement.Core.Interfaces;
using AutoClutch.Auto.Service.Interfaces;
using ContractTrackingManagement.Core.Models;

namespace ContractTrackingManagement.Core.Services.Tests
{
    [TestClass()]
    public class WorkOrderService_GetNewWorkOrderNumberShould
    {
        [TestMethod()]
        public void GetNewWorkOrderNumber()
        {
            // Arrange.
            var autoMocker = new MoqAutoMocker<WorkOrderService>();

            var workOrderService = autoMocker.ClassUnderTest;

            var contract = new Models.contract
            {
                contractNumber = "1324-ELE",
                workOrders = new List<Models.workOrder>
                    {
                        new Models.workOrder() { workOrderNumber = "001-1324-ELE-031915-NC" },
                        new Models.workOrder() { workOrderNumber = "002-1324-ELE-032015-NC" }
                    }
            };

            var mockContractService = Mock.Get(autoMocker.Get<IService<Models.contract>>());

            mockContractService.Setup(handler => handler.Find(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.Is<bool?>(i => true))).Returns(contract);

            var location = new Models.location { code = "NC" };

            var mockLocationService = Mock.Get(autoMocker.Get<IService<location>>());

            mockLocationService.Setup(handler => handler.Find(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.Is<bool?>(i => true))).Returns(location);

            // Act.
            var result = workOrderService.GetNewWorkOrderNumber(1, 1);

            // Assert.
            Assert.AreEqual("003-1324-ELE-" + DateTime.Today.ToString("MMddyy") + "-NC", result, "The work order number does not match up with the assumed output.");
        }
    }
}