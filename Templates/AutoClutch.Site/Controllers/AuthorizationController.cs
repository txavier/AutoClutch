using $safeprojectname$.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/authorize")]
    public class AuthorizeController : ApiController
    {
        private IAuthorizeService _AuthorizeService;

        public AuthorizeController(IAuthorizeService AuthorizeService)
        {
            _AuthorizeService = AuthorizeService;
        }

        public IHttpActionResult Get(bool? loginRequired, string permissionCheckType, string requiredPermissions, string uri, string parameters)
        {
            List<KeyValuePair<string, string>> parameterList = null;

            if (parameters != null)
            {
                parameters = parameters.Replace("\"", "");

                var nameValueCollection = HttpUtility.ParseQueryString(parameters);

                parameterList = new List<KeyValuePair<string, string>>();

                for (int index = 0; index < nameValueCollection.Count; index++)
                {
                    var key = nameValueCollection.Keys[index];
                    var value = nameValueCollection[index];

                    parameterList.Add(new KeyValuePair<string, string>(key, value));
                }
            }

            var result = _AuthorizeService.IsAuthorized(User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault(), loginRequired, permissionCheckType, requiredPermissions, uri, parameterList);

            return Ok(result);
        }

        [Route("getWithParams")]
        [HttpPost]
        public IHttpActionResult GetWithParams(bool? loginRequired, string permissionCheckType, string requiredPermissions, string uri, List<KeyValuePair<string, string>> parameters)
        {
            var result = _AuthorizeService.IsAuthorized(User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault(), loginRequired, permissionCheckType, requiredPermissions, uri, parameters);

            return Ok(result);
        }

    }
}
