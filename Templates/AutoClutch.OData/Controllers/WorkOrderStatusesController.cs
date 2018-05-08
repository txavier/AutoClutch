using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Interfaces;
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
    [RoutePrefix("api/workOrderStatuses")]
    public class WorkOrderStatusesController : BaseApiController<workOrderStatus>
    {
        private IService<workOrderStatus> _workOrderStatusService;

        public WorkOrderStatusesController(IService<workOrderStatus> workOrderStatusService)
            : base(workOrderStatusService)
        {
            _workOrderStatusService = workOrderStatusService;
        }

        [Route("count")]
        [HttpGet]
        public IHttpActionResult Count(string q = null)
        {
            var result = base.BaseCount(q);

            return Ok(result);
        }

    }
}
