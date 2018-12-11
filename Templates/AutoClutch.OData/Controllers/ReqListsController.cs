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
    public class ReqListsController : ODataApiController<ReqList>
    {
        public IService<ReqList> _ReqListService { get; set; } 

        public ReqListsController(IService<ReqList> ReqListService, ILogService<ReqList> logService)
            : base(ReqListService, logService)
        {
            _ReqListService = ReqListService;
        }
    }
}
