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
    public class Bulk_ChemicalsController : ODataApiController<Bulk_Chemicals>
    {
        public IService<Bulk_Chemicals> _Bulk_ChemicalsService { get; set; }

        public Bulk_ChemicalsController(IService<Bulk_Chemicals> Bulk_ChemicalsService, ILogService<Bulk_Chemicals> logService)
            : base(Bulk_ChemicalsService, logService)
        {
            _Bulk_ChemicalsService = Bulk_ChemicalsService;
        }
    }
}
