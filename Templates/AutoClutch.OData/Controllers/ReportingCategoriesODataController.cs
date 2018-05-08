using AutoClutch.Controller;
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
    public class ReportingCategoriesODataController : ODataApiController<reportingCategory>
    {
        private IService<reportingCategory> _reportingCategoryService;

        public ReportingCategoriesODataController(IService<reportingCategory> reportingCategoryService, ILogService<reportingCategory> logService)
            : base(reportingCategoryService, logService)
        {
            _reportingCategoryService = reportingCategoryService;
        }

    }
}
