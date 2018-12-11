using AutoClutch.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTPS.Core.Services;
using System.Web.OData;
using System.Web.OData.Routing;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;
using OTPS.Core.Models;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/users")]
    public class UserApiController : ApiController
    {
        private IUserApiService _userApiService;
        private IService<User> _userService;

        public UserApiController(IUserApiService userApiService, IService<User> userService)
        {
            _userApiService = userApiService;
            _userService = userService;
        }

        [HttpDelete]
        [Route("deleteUser(Key={Key})")]
        public void DeleteUser([FromUri] string Key)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _userApiService.deleteUser(Key, loggedInUserName);
        }

        [HttpGet] 
        [Route("getUserLevel()")]
        public int GetUserLevel()
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault().ToUpper();
            var result = _userService.Queryable().Where(i => i.UserId == loggedInUserName).Select(x => x.Level).FirstOrDefault();

            return result;
        }
    }
}