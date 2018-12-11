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
    public class Batch_RepCatsController : ODataApiController<Batch_RepCat>
    {
        public IService<Batch_RepCat> _Batch_RepCatService { get; set; }

        public Batch_RepCatsController(IService<Batch_RepCat>  Batch_RepCatService, ILogService<Batch_RepCat> logService)
            : base(Batch_RepCatService, logService)
        {
            _Batch_RepCatService = Batch_RepCatService; 
        }
    }
}
