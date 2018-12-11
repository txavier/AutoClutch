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
    public class contract_pay_historysController : ODataApiController<contract_pay_history>
    {
        public IService<contract_pay_history> _contract_pay_historyService { get; set; }

        public contract_pay_historysController(IService<contract_pay_history> contract_pay_historyService, ILogService<contract_pay_history> logService)
            : base(contract_pay_historyService, logService)
        {
            _contract_pay_historyService = contract_pay_historyService;
        }
    }
}
