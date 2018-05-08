using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.Core.Services;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/paymentTypes")]
    public class PaymentTypesController : BaseApiController<paymentType>
    {
        private IPaymentTypeService _paymentTypeService;

        public PaymentTypesController(IPaymentTypeService paymentTypeService)
            : base(paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }

        [Route("getPaymentTypeProjectRetainage")]
        [HttpGet]
        public IHttpActionResult GetPaymentTypeProjectRetainage(string paymentTypeName, double? projectRetainage = null)
        {
            var result = _paymentTypeService.GetPaymentTypeProjectRetainage(paymentTypeName, projectRetainage);

            return Ok(result);
        }

        [Route("getScopedPaymentTypes")]
        [HttpGet]
        public IHttpActionResult GetScopedPaymentTypes(string contractNumber)
        {
            _paymentTypeService.ProxyCreationEnabled = false;

            _paymentTypeService.LazyLoadingEnabled = false;

            var result = _paymentTypeService.GetScopedPaymentTypes(contractNumber);

            return Ok(result);
        }
    }
}