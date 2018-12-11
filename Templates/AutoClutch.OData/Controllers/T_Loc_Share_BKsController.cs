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
    public class T_Loc_Share_BKsController : ODataApiController<T_Loc_Share_BK>
    {
        public IService<T_Loc_Share_BK> _T_Loc_Share_BKService { get; set; }

        public T_Loc_Share_BKsController(IService<T_Loc_Share_BK> T_Loc_Share_BKService, ILogService<T_Loc_Share_BK> logService)
            : base(T_Loc_Share_BKService, logService)
        {
            _T_Loc_Share_BKService = T_Loc_Share_BKService;
        }
    }
}
