using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
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
    public class ContractorContactPersonsODataController : BaseApiController<contractorContactPerson>
    {
        private IService<contractorContactPerson> _contractorContactPersonService;

        public ContractorContactPersonsODataController(IService<contractorContactPerson> contractorContactPersonService, ILogService<contractorContactPerson> logService)
            : base(contractorContactPersonService, logService)
        {
            _contractorContactPersonService = contractorContactPersonService;
        }

    }
}
