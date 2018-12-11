using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Interfaces;
using OTPS.Core.Models;
using OTPS.Core.Objects;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace $safeprojectname$.Controllers
{
    public class ContractsController : ODataApiController<Contract>
    {
        public IService<Contract> _ContractService { get; set; }
        public IContractService _contractService;
        public IContractModifier _contractModifier;

        public ContractsController(IService<Contract> ContractService, IContractService contractService, IContractModifier contractModifier, ILogService<Contract> logService)
            : base(ContractService, logService)
        {
            _ContractService = ContractService;
            _contractService = contractService;
            _contractModifier = contractModifier;
        }

        [HttpPost]
        [ODataRoute("addContract")]
        public int addContract(ODataActionParameters parameters)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            var result = _contractModifier.addContract((Contract)parameters[typeof(Contract).Name], loggedInUserName);
            return result;
        }
    }
}
