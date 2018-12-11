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
    public class Multi_Year_GrandsController : ODataApiController<Multi_Year_Grand>
    {
        public IService<Multi_Year_Grand> _Multi_Year_GrandService { get; set; }

        public Multi_Year_GrandsController(IService<Multi_Year_Grand> Multi_Year_GrandService, ILogService<Multi_Year_Grand> logService)
            : base(Multi_Year_GrandService, logService)
        {
            _Multi_Year_GrandService = Multi_Year_GrandService;
        }
    }
}
