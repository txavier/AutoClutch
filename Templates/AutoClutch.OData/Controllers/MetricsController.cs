using AutoClutch.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace $safeprojectname$.Controllers
{
    //[RoutePrefix("api/metrics")]
    //public class MetricsController : ApiController
    //{
    //    private IMetricsService _metricsService;

    //    public MetricsController(IMetricsService metricsService)
    //    {
    //        _metricsService = metricsService;
    //    }

    //    [HttpGet]
    //    [Route("GetPermitsPerPlant")]
    //    public MetricsData GetPermitsPerPlant()
    //    {
    //        var result = _metricsService.GetPermitsPerPlant();

    //        return result;
    //    }

    //    [HttpGet]
    //    [Route("GetPermitsPerPlantLast90Days")]
    //    public MetricsData GetPermitsPerPlantLast90Days()
    //    {
    //        var result = _metricsService.GetPermitsPerPlantLast90Days();

    //        return result;
    //    }

    //    [HttpGet]
    //    [Route("GetPermitsPerPlantYearToDate")]
    //    public MetricsData GetPermitsPerPlantYearToDate()
    //    {
    //        var result = _metricsService.GetPermitsPerPlantYearToDate();

    //        return result;
    //    }

    //}
}
