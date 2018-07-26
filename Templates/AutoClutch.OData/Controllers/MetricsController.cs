using AutoClutch.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutchTemplate.Core.Services;
using System.Web.OData;
using System.Web.OData.Routing;
using AutoClutchTemplate.Core.Interfaces;
using AutoClutchTemplate.Core.Objects;

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
    }
}
