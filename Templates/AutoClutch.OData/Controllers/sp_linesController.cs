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
    public class sp_linesController : ODataApiController<sp_line>
    {
        public IService<sp_line> _sp_lineService { get; set; }

        public sp_linesController(IService<sp_line> sp_lineService, ILogService<sp_line> logService)
            : base(sp_lineService, logService)
        {
            _sp_lineService = sp_lineService;
        }


    }
}
