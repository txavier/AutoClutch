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
    //public class DashboardODataController : ODataController
    //{
    //    private IMetricService _metricService;

    //    public DashboardODataController(IMetricService metricService)
    //    {
    //        _metricService = metricService;
    //    }

    //    [ODataRoute("GetDashboardMetric(name={name},loggedInUserId={loggedInUserId})")]
    //    public IHttpActionResult GetDashboardMetric(string name, int? loggedInUserId = null)
    //    {
    //        switch (name)
    //        {
    //            case "contractTotalsPerSection":
    //                {
    //                    var result = _metricService.QueryContractTotalsPerSection();

    //                    return Ok(result);
    //                }
    //            default:
    //                {
    //                    break;
    //                }
    //        }

    //        return NotFound();
    //    }

    //}
}
