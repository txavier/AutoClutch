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
    public class ContractTypesController : BaseApiController<contractType>
    {
        private IService<contractType> _contractTypeService;

        public ContractTypesController(IService<contractType> contractTypeService)
            : base(contractTypeService)
        {
            _contractTypeService = contractTypeService;
        }

    }
}
