using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Interfaces;
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
    public class Mod_LogbooksController : ODataApiController<Mod_Logbook>
    {
        public IMod_LogbookService _Mod_LogbookService { get; set; }

        public Mod_LogbooksController(IMod_LogbookService Mod_LogbookService)
            : base(Mod_LogbookService)
        {
            _Mod_LogbookService = Mod_LogbookService;
        }
    }
}
