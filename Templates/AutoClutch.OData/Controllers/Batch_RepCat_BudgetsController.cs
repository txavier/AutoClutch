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
    public class Batch_RepCat_BudgetsController : ODataApiController<Batch_RepCat_Budget>
    {
        public IService<Batch_RepCat_Budget> _Batch_RepCat_BudgetService { get; set; }

        public Batch_RepCat_BudgetsController(IService<Batch_RepCat_Budget> Batch_RepCat_BudgetService, ILogService<Batch_RepCat_Budget> logService)
            : base(Batch_RepCat_BudgetService, logService)
        {
            _Batch_RepCat_BudgetService = Batch_RepCat_BudgetService;
        }
    }
}
