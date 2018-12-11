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
    public class T_RecsController : ODataApiController<T_Rec>
    {
        public IService<T_Rec> _T_RecService { get; set; }

        public T_RecsController(IService<T_Rec> T_RecService, ILogService<T_Rec> logService)
            : base(T_RecService, logService)
        {
            _T_RecService = T_RecService;
        }
    }
}
