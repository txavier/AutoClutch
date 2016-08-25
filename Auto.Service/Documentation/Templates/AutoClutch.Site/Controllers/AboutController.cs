using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/about")]
    public class AboutController : ApiController
    {
        private readonly IConfigSettingsGetter _configSettingsGetter;

        public AboutController(IConfigSettingsGetter configSettingsGetter)
        {
            _configSettingsGetter = configSettingsGetter;
        }

        [Route("getVersion")]
        [HttpGet]
        public IHttpActionResult getVersion()
        {
            var result = _configSettingsGetter.GetVersion();

            return Ok(result);
        }

    }
}
