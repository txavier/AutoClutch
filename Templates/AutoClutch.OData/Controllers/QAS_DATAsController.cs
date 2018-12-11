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
    public class QAS_DATAsController : ODataApiController<QAS_DATA>
    {
        public IService<QAS_DATA> _QAS_DATAService { get; set; }

        public QAS_DATAsController(IService<QAS_DATA> QAS_DATAService, ILogService<QAS_DATA> logService)
            : base(QAS_DATAService, logService)
        {
            _QAS_DATAService = QAS_DATAService;
        }
    }
}
