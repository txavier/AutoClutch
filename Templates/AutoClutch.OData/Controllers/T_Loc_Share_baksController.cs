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
    public class T_Loc_Share_baksController : ODataApiController<T_Loc_Share_bak>
    {
        public IService<T_Loc_Share_bak> _T_Loc_Share_bakService { get; set; }

        public T_Loc_Share_baksController(IService<T_Loc_Share_bak> T_Loc_Share_bakService, ILogService<T_Loc_Share_bak> logService)
            : base(T_Loc_Share_bakService, logService)
        {
            _T_Loc_Share_bakService = T_Loc_Share_bakService;
        }
    }
}
