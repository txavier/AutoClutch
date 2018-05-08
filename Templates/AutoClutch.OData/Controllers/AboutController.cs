using $safeprojectname$.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/about")]
    public class AboutController : ApiController
    {
        private IEnvironmentConfigSettingsGetter _environmentConfigSettingsGetter;

        public AboutController(IEnvironmentConfigSettingsGetter environmentConfigSettingsGetter)
        {
            _environmentConfigSettingsGetter = environmentConfigSettingsGetter;
        }

        [Route("getVersion")]
        [HttpGet]
        public IHttpActionResult getVersion()
        {
            var result = _environmentConfigSettingsGetter.GetVersion();

            return Ok(result);
        }
    }
}