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
    public class Ref_DocumentsController : ODataApiController<Ref_Document>
    {
        public IService<Ref_Document> _Ref_DocumentService { get; set; }

        public Ref_DocumentsController(IService<Ref_Document> Ref_DocumentService, ILogService<Ref_Document> logService)
            : base(Ref_DocumentService, logService)
        {
            _Ref_DocumentService = Ref_DocumentService;
        }
    }
}
