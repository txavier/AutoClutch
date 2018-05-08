using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace $safeprojectname$.Controllers
{
    public class TimelineItemsController : ODataApiController<timelineItem>
    {
        private ITimelineItemService _timelineItemService;

        private ILogService<timelineItem> _logService;

        public TimelineItemsController(ITimelineItemService timelineItemService, ILogService<timelineItem> logService)
            : base(timelineItemService, logService)
        {
            _timelineItemService = timelineItemService;

            _logService = logService;
        }

        [HttpGet]
        [ODataRoute("ConcatenatePreviousFiles(projectId={projectId})")]
        public IHttpActionResult ConcatenatePreviousFiles([FromODataUri]int projectId)
        {
            var result = _timelineItemService.ConcatenatePreviousFiles(projectId, null, User.Identity.Name.Split('\\').LastOrDefault());

            return Ok(result);
        }
    }
}
