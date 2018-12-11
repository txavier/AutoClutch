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
    public class T_LOCsController : ODataApiController<T_LOC>
    {
        public IService<T_LOC> _T_LOCService { get; set; }

        public T_LOCsController(IService<T_LOC> T_LOCService, ILogService<T_LOC> logService)
            : base(T_LOCService, logService)
        {
            _T_LOCService = T_LOCService;
        }
    }
}
