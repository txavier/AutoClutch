using System.Web.Http;
using OTPS.Core.Services;
using System.Web.OData;
using System.Web.OData.Routing;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers
{
	[RoutePrefix("api/currentcommited")]
	public class CurrentCommitedController : ApiController
	{
		private ICurrentCommitedService _currentcommitedService;

		public CurrentCommitedController(ICurrentCommitedService currentCommitedService)
		{
			_currentcommitedService = currentCommitedService;
		}

		[HttpGet]
		[Route("getCurrentCommited")]
		public CurrentCommitedData GetCurrentCommited()
		{
			var result = _currentcommitedService.getCurrentCommited();

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