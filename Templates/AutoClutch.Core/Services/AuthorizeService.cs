using AutoClutch.Core.Interfaces;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClutch.Core.Interfaces;

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
            var isAuthorized = "notAuthorized";

            var engineer = _engineerService.Queryable().FirstOrDefault(i => i.userName == userName);

            // If the person is not in the database then they are not authorized
            // to make any changes.
            if (engineer == null)
            {
                return "notAuthorized";
            }

            var engineerRoles = new List<string>();

            if (engineer.adminRole ?? false)
            {
                engineerRoles.Add("admin");
            }

            if (engineer.engineerRole ?? false)
            {
                engineerRoles.Add("engineer");
            }

            if (requiredPermissions.Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Intersect(engineerRoles).Any())
            {
                // If there are uri parameters available...
                if (parameters != null && parameters.Any())
                {
                    if (engineerRoles.Contains("engineer"))
                    {
                        // If there is any intersection between the contract numbers of this engineers contracts 
                        // and the parameters contract numbers that are returned then this user has read and write 
                        // access to this contract.
                        if (engineerRoles.Contains("admin"))
                        {
                            isAuthorized = "authorized";
                        }
                        else
                        {
                            isAuthorized = "readOnly";
                        }
                    }
                }

                // If this user is an admin and one of the required permission is an admin
                // then allow whatever action they want.
                if (engineerRoles.Contains("admin"))
                {
                    if (engineer.adminRole ?? false)
                    {
                        isAuthorized = "authorized";
                    }
                    else
                    {
                        isAuthorized = "notAuthorized";
                    }
                }

                if (engineerRoles.Contains("engineer"))
                {
                    isAuthorized = "authorized";
                }
            }

            return isAuthorized;
        }

    }
}
