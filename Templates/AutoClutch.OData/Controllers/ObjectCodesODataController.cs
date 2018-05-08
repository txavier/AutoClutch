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
    public class ObjectCodesODataController : ODataApiController<objectCode>
    {
        private IService<objectCode> _objectCodeService;

        public ObjectCodesODataController(IService<objectCode> objectCodeService, ILogService<objectCode> logService)
            : base(objectCodeService, logService)
        {
            _objectCodeService = objectCodeService;
        }

    }
}
