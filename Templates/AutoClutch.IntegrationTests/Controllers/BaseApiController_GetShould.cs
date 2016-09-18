using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractTrackingManagement.DependencyResolution;
using AutoClutch.Auto.Service.Interfaces;
using ContractTrackingManagement.Core.Models;
using System.Web.Http.Results;
using ContractTrackingManagement.Core.Interfaces;

namespace $safeprojectname$
{
    [TestClass()]
    public class BaseApiController_GetShould
    {
        /// <summary>
        /// https://thesoftwaredudeblog.wordpress.com/2014/01/19/using-ihttpactionresult-with-webapi2/
        /// </summary>
        [TestMethod()]
        public void GetContractWithCalculatedFields()
        {
            // Arrange.
            var container = IoC.Initialize();

            var contractService = container.GetInstance<IContractService>();

            var controller = new ContractsController(contractService, null);

            // Act.
            IHttpActionResult result = controller.Get("contractId", "type,contractType", null, null, 1, 1);

            // Assert.
            Assert.IsNotNull(result);
        }
    }
}