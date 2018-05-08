using AutoClutch.Controller;
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
    public class WorkOrdersODataController : ODataApiController<workOrder>
    {
        private IWorkOrderService _workOrderService;

        public WorkOrdersODataController(IWorkOrderService workOrderService, ILogService<workOrder> logService)
            : base(workOrderService, logService)
        {
            _workOrderService = workOrderService;
        }

        //copied
        public IHttpActionResult GetNewWorkOrderNumber(int contractId, int locationId)
        {
            var result = _workOrderService.GetNewWorkOrderNumber(contractId, locationId);

            // Get any errors.
            var errors = _workOrderService.Errors;

            if (errors.Any())
            {
                return RetrieveErrorResult(errors);
            }

            return Ok(result);
        }

        //copied
        public IHttpActionResult GetInitialWorkOrderEmail(string workOrderNumber = null, int? contractId = null)
        {
            var result = string.IsNullOrEmpty(workOrderNumber) ? _workOrderService.GetInitialWorkOrderEmail(contractId) :
                _workOrderService.GetInitialWorkOrderEmail(workOrderNumber);

            // Get any errors.
            var errors = _workOrderService.Errors;

            if (errors.Any())
            {
                return RetrieveErrorResult(errors);
            }

            return Ok(result);
        }

        //copied
        public IHttpActionResult SendWorkOrderEmail(email email)
        {
            try
            {
                _workOrderService.SendWorkOrderEmail(email, User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault());

                // Get any errors.
                var errors = _workOrderService.Errors;

                if (errors.Any())
                {
                    return RetrieveErrorResult(errors);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
