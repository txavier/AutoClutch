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
    public class ReqList_BudgetsController : ODataApiController<ReqList_Budget>
    {
        public IService<ReqList_Budget> _ReqList_BudgetService { get; set; }

        public ReqList_BudgetsController(IService<ReqList_Budget> ReqList_BudgetService, ILogService<ReqList_Budget> logService)
            : base(ReqList_BudgetService, logService)
        {
            _ReqList_BudgetService = ReqList_BudgetService;
        }
    }
}
