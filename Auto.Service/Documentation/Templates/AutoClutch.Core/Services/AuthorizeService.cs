using AutoClutch.Auto.Core.Interfaces;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    /// <summary>
    /// http://jonsamwell.com/url-route-authorization-and-security-in-angular/#interception
    /// </summary>
    public class AuthorizeService : IAuthorizeService
    {
        private IService<user> _engineerService;

        public AuthorizeService(IService<user> engineerService)
        {
            _engineerService = engineerService;
        }

        public string IsAuthorized(string userName, bool? loginRequired, string permissionCheckType, string requiredPermissions, string uri,
            List<KeyValuePair<string, string>> parameters)
        {
            //isAuthorized = "readOnly";
            //isAuthorized = "authorized";
            //isAuthorized = "notAuthorized";

            return "authorized";
        }

    }
}
