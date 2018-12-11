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
    //public class UsersController : ODataApiController<user>
    //{
    //    public IService<user> _userService { get; set; }

    //    public UsersController(IService<user> userService, ILogService<user> logService)
    //        : base(userService, logService)
    //    {
    //        _userService = userService;
    //    }

    //    [ODataRoute("GetLoggedInUser()")]
    //    public IHttpActionResult GetLoggedInUser()
    //    {
    //        var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

    //        return Ok(loggedInUserName);
    //    }

    //}
}
