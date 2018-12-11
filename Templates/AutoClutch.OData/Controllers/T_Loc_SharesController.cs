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
    public class T_Loc_SharesController : ODataApiController<T_Loc_Share>
    {
        public IService<T_Loc_Share> _T_Loc_ShareService { get; set; }

        public T_Loc_SharesController(IService<T_Loc_Share> T_Loc_ShareService, ILogService<T_Loc_Share> logService)
            : base(T_Loc_ShareService, logService)
        {
            _T_Loc_ShareService = T_Loc_ShareService;
        }
    }
}
