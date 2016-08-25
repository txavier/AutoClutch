using AutoClutch.Auto.Service.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        private readonly IMetricService _metricService;

        public DashboardController(IMetricService metricService)
        {
            _metricService = metricService;
        }

        [HttpGet]
        public IHttpActionResult Get(string name, int? loggedInUserId = null)
        {
            switch (name)
            {
                case "contractTotalsPerSection":
                    {
                        var result = _metricService.QueryContractTotalsPerSection();

                        return Ok(result);
                    }
                default:
                    {
                        break;
                    }
            }

            return NotFound();
        }

    }
}
