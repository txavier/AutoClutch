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
    public class WorkOrderStatusesODataController : ODataApiController<workOrderStatus>
    {
        private IService<workOrderStatus> _workOrderStatusService;

        public WorkOrderStatusesODataController(IService<workOrderStatus> workOrderStatusService)
            : base(workOrderStatusService)
        {
            _workOrderStatusService = workOrderStatusService;
        }

    }
}
