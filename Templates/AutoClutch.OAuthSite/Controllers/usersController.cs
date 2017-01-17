using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutch.Core.Interfaces;
using AutoClutch.Controller;

namespace $safeprojectname$.Controllers
{
    public class usersController : ODataApiController<user>
    {
        private IService<user> _userService;

        public usersController(IService<user> userService)
            : base(userService)
        {
            _userService = userService;
        }

        public IHttpActionResult GetLoggedInUser()
        {
            return Ok(User?.Identity?.Name ?? "");
        }
    }
}