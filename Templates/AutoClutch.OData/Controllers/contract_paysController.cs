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
    public class contract_paysController : ODataApiController<contract_pay>
    {
        public IService<contract_pay> _contract_payService { get; set; }

        public contract_paysController(IService<contract_pay> contract_payService, ILogService<contract_pay> logService)
            : base(contract_payService, logService)
        {
            _contract_payService = contract_payService;
        }
    }
}
