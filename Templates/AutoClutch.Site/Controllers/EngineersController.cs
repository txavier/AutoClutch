using AutoClutch.Auto.Service.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/engineers")]
    public class EngineersController : BaseApiController<engineer>
    {
        private IService<engineer> _engineerService;

        private ILogService<engineer> _logService;

        public EngineersController(IService<engineer> engineerService, ILogService<engineer> logService)
            : base(engineerService, logService)
        {
            _engineerService = engineerService;

            _logService = logService;
        }

        [Route("getEngineers")]
        [HttpGet]
        public IHttpActionResult GetEngineers()
        {
            _engineerService.LazyLoadingEnabled = false;

            _engineerService.ProxyCreationEnabled = false;

            var result = _engineerService.Queryable();

            return Ok(result);
        }

        [Route("getLoggedInUser")]
        [HttpGet]
        public IHttpActionResult GetLoggedInUser()
        {
            _engineerService.LazyLoadingEnabled = false;

            _engineerService.ProxyCreationEnabled = false;

            var userName = User.Identity.Name.Split('\\').LastOrDefault();

            var user = _engineerService.Queryable().SingleOrDefault(i => i.userName.Equals(userName));

            return Ok(user);
        }

    }
}
