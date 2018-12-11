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
    public class T_Loc_BudgetsController : ODataApiController<T_Loc_Budget>
    {
        public IService<T_Loc_Budget> _T_Loc_BudgetService { get; set; }

        public T_Loc_BudgetsController(IService<T_Loc_Budget> T_Loc_BudgetService, ILogService<T_Loc_Budget> logService)
            : base(T_Loc_BudgetService, logService)
        {
            _T_Loc_BudgetService = T_Loc_BudgetService;
        }
    }
}
