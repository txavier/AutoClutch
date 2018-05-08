using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Models;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/userActionLogs")]
    public class UserActionLogsController : BaseApiController<userActionLog>
    {
        private IContractUserActionLogService<userActionLog> _contractUserActionLogService;

        public UserActionLogsController(IContractUserActionLogService<userActionLog> contractUserActionLogService, ILogService<userActionLog> userActionLogService)
            : base((IService<userActionLog>)contractUserActionLogService, userActionLogService)
        {
            _contractUserActionLogService = contractUserActionLogService;
        }

        public string GetLoggedInUserName(IPrincipal User)
        {
            if(User?.Identity?.Name == null)
            {
                return null;
            }

            var result = User.Identity.Name.Split("\\".ToCharArray()).Last();

            return result;
        }

        [Route("getLatestUserActionLogs")]
        [HttpGet]
        public IHttpActionResult GetLatestUserActionLogs(int take)
        {
            var result = _contractUserActionLogService.GetLatestUserActionLogs(take, GetLoggedInUserName(User));

            return Ok(result);
        }

        [Route("getRestrictedUserActionLogs")]
        [HttpGet]
        public virtual IHttpActionResult GetRestrictedUserActionLogs(
            string sort = null,
            int page = 1,
            int perPage = Int32.MaxValue)
        {
            var result = _contractUserActionLogService.GetRestrictedUserActionLogs(sort, page, perPage, User.Identity.Name.Split("\\".ToCharArray()).Last());

            return Ok(result);
        }

        [Route("getRestrictedUserActionLogsCount")]
        [HttpGet]
        public virtual IHttpActionResult GetRestrictedUserActionLogsCount()
        {
            var result = _contractUserActionLogService.GetRestrictedUserActionLogsCount(User.Identity.Name.Split("\\".ToCharArray()).Last());

            return Ok(result);
        }

    }
}
