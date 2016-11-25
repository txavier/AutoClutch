using Auto.Controller.Objects;
using WebX.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutch.Core.Interfaces;

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
            var user = _userService.Queryable().FirstOrDefault(i => i.userName == User.Identity.Name);

            return Ok(user);
        }
    }
}