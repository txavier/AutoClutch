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
    public class Ref_ObjectChemicalCodesController : ODataApiController<Ref_ObjectChemicalCode>
    {
        public IService<Ref_ObjectChemicalCode> _Ref_ObjectChemicalCodeService { get; set; }

        public Ref_ObjectChemicalCodesController(IService<Ref_ObjectChemicalCode> Ref_ObjectChemicalCodeService, ILogService<Ref_ObjectChemicalCode> logService)
            : base(Ref_ObjectChemicalCodeService, logService)
        {
            _Ref_ObjectChemicalCodeService = Ref_ObjectChemicalCodeService;
        }
    }
}
