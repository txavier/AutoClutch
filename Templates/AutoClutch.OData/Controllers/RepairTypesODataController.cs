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
    public class RepairTypesODataController : ODataApiController<repairType>
    {
        private IService<repairType> _repairTypeService;

        public RepairTypesODataController(IService<repairType> repairTypeService)
            : base(repairTypeService)
        {
            _repairTypeService = repairTypeService;
        }

    }
}
