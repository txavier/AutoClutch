using System.Web.Http;
using OTPS.Core.Services;
using System.Web.OData;
using System.Web.OData.Routing;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers
{
	[RoutePrefix("api/totalinitialallocation")]
	public class TotalInitialAllocationController : ApiController
	{
		private ITotalInitialAllocationService _totalInitialAllocationService;

		public TotalInitialAllocationController(ITotalInitialAllocationService totalInitialAllocationService)
		{
			_totalInitialAllocationService = totalInitialAllocationService;
		}

		[HttpGet]
		[Route("getTotalInitialAllocation")]
		public TotalInitialAllocationData GetTotalInitialAllocation()
		{
			var result = _totalInitialAllocationService.getTotalInitialAllocation();

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