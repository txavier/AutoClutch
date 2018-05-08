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
    public class ContractorsODataController : ODataApiController<contractor>
    {
        private IService<contractor> _contractorService;

        public ContractorsODataController(IService<contractor> contractorService, ILogService<contractor> logService)
            : base(contractorService, logService)
        {
            _contractorService = contractorService;
        }

    }
}
