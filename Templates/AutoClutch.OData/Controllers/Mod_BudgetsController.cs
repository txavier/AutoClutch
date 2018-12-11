using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace $safeprojectname$.Controllers
{
    public class Mod_BudgetsController : ODataApiController<Mod_Budget>
    {
        public IService<Mod_Budget> _Mod_BudgetService { get; set; }

        public Mod_BudgetsController(IService<Mod_Budget> Mod_BudgetService, ILogService<Mod_Budget> logService)
            : base(Mod_BudgetService, logService)
        {
            _Mod_BudgetService = Mod_BudgetService;
        }
    }
}
