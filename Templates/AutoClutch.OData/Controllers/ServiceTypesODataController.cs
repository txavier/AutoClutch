using AutoClutch.Core.Interfaces;
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
    public class ServiceTypesODataController : ODataApiController<serviceType>
    {
        private IService<serviceType> _serviceTypeService;

        public ServiceTypesODataController(IService<serviceType> serviceTypeService)
            : base(serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }

    }
}
