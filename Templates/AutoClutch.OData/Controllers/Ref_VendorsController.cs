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
    public class Ref_VendorsController : ODataApiController<Ref_Vendor>
    {
        public IService<Ref_Vendor> _Ref_VendorService { get; set; }

        public Ref_VendorsController(IService<Ref_Vendor> Ref_VendorService, ILogService<Ref_Vendor> logService)
            : base(Ref_VendorService, logService)
        {
            _Ref_VendorService = Ref_VendorService;
        }
    }
}
