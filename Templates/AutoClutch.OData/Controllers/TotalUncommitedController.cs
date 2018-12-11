using System.Web.Http;
using OTPS.Core.Services;
using System.Web.OData;
using System.Web.OData.Routing;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/totaluncommited")]
    public class TotalUncommitedController : ApiController
    {
        private ITotalUncommitedService _totalUncommitedService;

        public TotalUncommitedController(ITotalUncommitedService totalUncommitedService)
        {
            _totalUncommitedService = totalUncommitedService;
        }

        [HttpGet]
        [Route("getTotalUncommited")]
        public TotalUncommitedData GetTotalUncommited()
        {
            var result = _totalUncommitedService.getTotalUncommited();

            return result;
        }

        [Route("getVersion")]
        [HttpGet]
        public IHttpActionResult getVersion()
        {
            //var result = _environmentConfigSettingsGetter.GetVersion();
            var result = "-1.0";

            return Ok(result);
        }
    }
}