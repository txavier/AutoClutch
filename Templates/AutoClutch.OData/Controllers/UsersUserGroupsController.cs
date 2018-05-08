using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
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
    public class userUserGroupsController : ODataApiController<userUserGroup>
    {
        private IService<userUserGroup> _userUserGroupService;

        public userUserGroupsController(IService<userUserGroup> userUserGroupService, ILogService<userUserGroup> logService)
            : base(userUserGroupService, logService)
        {
            _userUserGroupService = userUserGroupService;
        }
    }
}
