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
    public class UsersController : ODataApiController< User >
    {
        public IService<User> _UsersService { get; set; }

        public UsersController(IService<User> UsersService, ILogService<User> logService)
            : base(UsersService, logService)
        {
            _UsersService = UsersService;
        }

        [ODataRoute("GetLoggedInUser()")]
        public IHttpActionResult GetLoggedInUser()
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            return Ok(loggedInUserName);
        }
    }
}
