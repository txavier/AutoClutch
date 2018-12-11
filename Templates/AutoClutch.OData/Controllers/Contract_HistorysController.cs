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
    public class Contract_HistorysController : ODataApiController<Contract_History>
    {
        public IService<Contract_History> _Contract_HistoryService { get; set; }

        public Contract_HistorysController(IService<Contract_History> Contract_HistoryService,  ILogService<Contract_History> logService)
            : base(Contract_HistoryService, logService)
        {
            _Contract_HistoryService = Contract_HistoryService;
        }
    }
}
