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
    public class Ref_ObjectAllocTypesController : ODataApiController<Ref_ObjectAllocType>
    {
        public IService<Ref_ObjectAllocType> _Ref_ObjectAllocTypeService { get; set; }

        public Ref_ObjectAllocTypesController(IService<Ref_ObjectAllocType> Ref_ObjectAllocTypeService, ILogService<Ref_ObjectAllocType> logService)
            : base(Ref_ObjectAllocTypeService, logService)
        {
            _Ref_ObjectAllocTypeService = Ref_ObjectAllocTypeService;
        }
    }
}
