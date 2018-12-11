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
    public class Modi_BatchsController : ODataApiController<Modi_Batch>
    {
        public IService<Modi_Batch> _Modi_BatchService { get; set; }

        public Modi_BatchsController(IService<Modi_Batch> Modi_BatchService, ILogService<Modi_Batch> logService)
            : base(Modi_BatchService, logService)
        {
            _Modi_BatchService = Modi_BatchService;
        }
    }
}
