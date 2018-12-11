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

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/metrics")]
    public class MetricsController : ApiController
    {
        private IMetricsService _metricsService;

        public MetricsController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        [HttpGet]
        [Route("getTopFiveBudgetCodes")]
        public MetricsData GetTopFiveBudgetCodes()
        {
            var result = _metricsService.getTopFiveBudgetCodes();

            return result;
        }

		[HttpGet]
		[Route("getBottomFiveBudgetCodes")]
		public MetricsData GetBottomFiveBudgetCodes()
		{
			var result = _metricsService.getBottomFiveBudgetCodes();

			return result;
		}
	}
}
