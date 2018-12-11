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
    public class Ref_OrderTypesController : ODataApiController<Ref_OrderType>
    {
        public IService<Ref_OrderType> _Ref_OrderTypeService { get; set; }

        public Ref_OrderTypesController(IService<Ref_OrderType> Ref_OrderTypeService, ILogService<Ref_OrderType> logService)
            : base(Ref_OrderTypeService, logService)
        {
            _Ref_OrderTypeService = Ref_OrderTypeService;
        }
    }
}
