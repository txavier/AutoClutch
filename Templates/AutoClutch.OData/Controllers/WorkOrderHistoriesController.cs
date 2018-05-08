using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Interfaces;
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
    [RoutePrefix("api/workOrderHistories")]
    public class WorkOrderHistoriesController : BaseApiController<workOrderHistory>
    {
        private IService<workOrderHistory> _workOrderHistoryService;

        public WorkOrderHistoriesController(IService<workOrderHistory> workOrderHistoryService, ILogService<workOrderHistory> logService)
            : base(workOrderHistoryService, logService)
        {
            _workOrderHistoryService = workOrderHistoryService;
        }

    }
}
