using $safeprojectname$.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/histories")]
    public class HistoriesController : ApiController
    {
        private ITrackerEnabledService _trackerEnabledService;

        public HistoriesController(ITrackerEnabledService trackerEnabledService)
        {
            _trackerEnabledService = trackerEnabledService;
        }

        public IHttpActionResult Get(string typeFullName, int id)
        {
            var result = _trackerEnabledService.Get(typeFullName, id);

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult Get(int? page, int? perPage)
        {
            var result = _trackerEnabledService.Get(page, perPage);

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetLogDetails(string typeFullName, int id, string propertyName)
        {
            var result = _trackerEnabledService.GetLogDetails(typeFullName, id, propertyName);

            return Ok(result);
        }


        [HttpGet]
        public IHttpActionResult Count(bool count = true)
        {
            var result = _trackerEnabledService.GetCountAll();

            return Ok(result);
        }

    }
}
