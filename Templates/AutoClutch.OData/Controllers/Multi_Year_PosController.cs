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
    public class Multi_Year_PosController : ODataApiController<Multi_Year_Po>
    {
        public IService<Multi_Year_Po> _Multi_Year_PoService { get; set; }

        public Multi_Year_PosController(IService<Multi_Year_Po> Multi_Year_PoService, ILogService<Multi_Year_Po> logService)
            : base(Multi_Year_PoService, logService)
        {
            _Multi_Year_PoService = Multi_Year_PoService;
        }
    }
}
