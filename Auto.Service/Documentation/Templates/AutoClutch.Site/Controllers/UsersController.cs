using AutoClutch.Auto.Core.Interfaces;
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
    [RoutePrefix("api/users")]
    public class UsersController : BaseApiController<user>
    {
        private IService<user> _userService;

        private ILogService<user> _logService;

        public UsersController(IService<user> userService, ILogService<user> logService)
            : base(userService, logService)
        {
            _userService = userService;

            _logService = logService;
        }

        [Route("getUsers")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            _userService.LazyLoadingEnabled = false;

            _userService.ProxyCreationEnabled = false;

            var result = _userService.Queryable();

            return Ok(result);
        }

        [Route("getLoggedInUser")]
        [HttpGet]
        public IHttpActionResult GetLoggedInUser()
        {
            _userService.LazyLoadingEnabled = false;

            _userService.ProxyCreationEnabled = false;

            var userName = User.Identity.Name.Split('\\').LastOrDefault();

            var user = _userService.Queryable().SingleOrDefault(i => i.userName.Equals(userName));

            return Ok(user);
        }

    }
}
