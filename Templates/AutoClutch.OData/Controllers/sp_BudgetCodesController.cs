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
using OTPS.Core.Interfaces;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("odata")]
    public class sp_BudgetCodesController : ODataApiController<sp_BudgetCode>
    {
        public IService<sp_BudgetCode> _sp_BudgetCodeService { get; set; }
        public ISpBudgetCodeService _sp_BCS { get; set; }

        public sp_BudgetCodesController(IService<sp_BudgetCode> sp_BudgetCodeService, ILogService<sp_BudgetCode> logService, ISpBudgetCodeService sp_BCS)
            : base(sp_BudgetCodeService, logService)
        {
            _sp_BudgetCodeService = sp_BudgetCodeService;
            _sp_BCS = sp_BCS;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("SelectBudObj")]
        public String[] SelectBudObj()
        {
            var result = _sp_BCS.SelectBudObj();

            return result;
        }
    }
}
