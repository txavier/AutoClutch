using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Objects;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace $safeprojectname$.Controllers
{
    public class ContractsODataController : ODataApiController<contract>
    {
        private IContractService _contractService;
        private readonly ILogService<contract> _logService;
        public ContractsODataController(IContractService contractService, ILogService<contract> logService)
            : base(contractService, logService)
        {
            _contractService = contractService;
        }

        [HttpGet]
        [ODataRoute("contractsOData/contractService.GetContractId(contractNumber={contractNumber})")]
        public IHttpActionResult GetContractId(string contractNumber)
        {
            var result = _contractService.Queryable().Where(i => i.contractNumber == contractNumber);

            if (result.Count() > 1)
            {
                return base.RetrieveErrorResult(new List<AutoClutch.Core.Objects.Error> { new AutoClutch.Core.Objects.Error { Description = "There are more than 1 " + contractNumber + " contracts in the database." } });
            }
            else if (!result.Any())
            {
                return Ok();
            }

            return Ok(result.FirstOrDefault().contractId);
        }

        [HttpGet]
        [ODataRoute("contractsOData/contractService.GetInitialContract(sectionName={sectionName})")]
        public IHttpActionResult GetInitialContract([FromODataUri] string sectionName)
        {
            var result = _contractService.GetInitialContract(sectionName);

            return Ok(result);
        }



        // Copied
        public IHttpActionResult GetInitialRenewalContract(string originalContractNumber)
        {
            var result = _contractService.GetInitialRenewalContract(originalContractNumber);

            return Ok(result);
        }

        // Copied
        public IHttpActionResult AddRenewalContract(contract contract)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _contractService.AddRenewalContract(contract, loggedInUserName: User.Identity.Name.Split("\\".ToCharArray()).Last(), lazyLoadingEnabled: false, proxyCreationEnabled: false);

                // Get any errors.
                var errors = _contractService.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                if (_logService != null)
                {
                    _logService.Info(contract, contract.contractId, EventType.Added, entityName: contract.contractNumber, loggedInUserName: User.Identity.Name.Split("\\".ToCharArray()).Last());
                }

                // Null is passed because the entity coming back from the service layer is 
                // filled with proxies and stuff that causes havok for the JSON deserializer.
                // Until this is fixed the return value should remain something that does 
                // not cause the deserializer to take a significantly long time.
                return Ok(new { contract.contractId, contract.contractNumber });
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);

                return InternalServerError(ex);
            }
        }
    }

}
