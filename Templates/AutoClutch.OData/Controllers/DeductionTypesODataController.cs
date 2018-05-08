using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.Core.Services;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("odata/deductionTypes")]
    public class DeductionTypesODataController : ODataApiController<deductionType>
    {
        private IService<deductionType> _deductionTypeService;

        public DeductionTypesODataController(IService<deductionType> deductionTypeService)
            : base(deductionTypeService)
        {
            _deductionTypeService = deductionTypeService;
        }
    }
}