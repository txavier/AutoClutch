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
using TrackerEnabledDbContext.Common.Models;

namespace $safeprojectname$.Controllers
{
    public class AuditLogsController : ODataApiController<AuditLog>
    {
        public IService<AuditLog> _AuditLogsService { get; set; }

        public AuditLogsController(IService<AuditLog> AuditLogsService, ILogService<AuditLog> logService)
            : base(AuditLogsService, logService)
        {
            _AuditLogsService = AuditLogsService;
        }
    }
}
