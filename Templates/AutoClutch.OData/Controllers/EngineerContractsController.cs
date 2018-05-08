using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/engineerContracts")]
    public class EngineerContractsController : BaseApiController<engineerContract>
    {
        private IService<engineerContract> _engineerContractService;

        public EngineerContractsController(IService<engineerContract> engineerContractService)
            : base(engineerContractService)
        {
            _engineerContractService = engineerContractService;
        }

    }
}
