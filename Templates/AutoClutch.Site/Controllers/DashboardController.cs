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
        public DashboardController()
        {
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok();
        }

    }
}
