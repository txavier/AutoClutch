using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.Core.Services;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutch.Core.Models;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/userActionLogs")]
    public class UserActionLogsController : BaseApiController<userActionLog>
    {
        private readonly ILogService<userActionLog> _userActionLogService;

        public UserActionLogsController(ILogService<userActionLog> userActionLogService)
            : base((IService<userActionLog>)userActionLogService)
        {
            _userActionLogService = userActionLogService;
        }

        [Route("getLatestUserActionLogs")]
        [HttpGet]
        public IHttpActionResult GetLatestUserActionLogs(int take)
        {
            var result = _userActionLogService.GetLatestUserActionLogs(take);

            return Ok(result);
        }

    }
}
