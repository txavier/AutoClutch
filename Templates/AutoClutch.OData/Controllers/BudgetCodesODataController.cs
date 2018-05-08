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
    public class BudgetCodesODataController : ODataApiController<budgetCode>
    {
        private IService<budgetCode> _budgetCodeService;

        public BudgetCodesODataController(IService<budgetCode> budgetCodeService, ILogService<budgetCode> logService)
            : base(budgetCodeService, logService)
        {
            _budgetCodeService = budgetCodeService;
        }

    }
}
