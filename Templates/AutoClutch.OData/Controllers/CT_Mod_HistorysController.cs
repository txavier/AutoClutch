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
    public class CT_Mod_HistorysController : ODataApiController<CT_Mod_History>
    {
        public IService<CT_Mod_History> _CT_Mod_HistoryService { get; set; }

        public CT_Mod_HistorysController(IService<CT_Mod_History> CT_Mod_HistoryService, ILogService<CT_Mod_History> logService)
            : base(CT_Mod_HistoryService, logService)
        {
            _CT_Mod_HistoryService = CT_Mod_HistoryService;
        }


     
    }
}
