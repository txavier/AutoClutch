using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/deductions")]
    public class DeductionsController : BaseApiController<deduction>
    {
        private IDeductionService _deductionService;

        public DeductionsController(IDeductionService deductionService, ILogService<deduction> logService)
            : base(deductionService, logService)
        {
            _deductionService = deductionService;
        }

        [Route("getDeductionsByContractNumber")]
        [HttpGet]
        public IHttpActionResult GetDeductionsByContractNumber(string contractNumber, string sort = null, string expand = null, string fields = null, string q = null, int page = 1, int perPage = Int32.MaxValue)
        {
            var skip = (page - 1) * perPage;

            // If sort is descending then change the '-' sign which represents the descending order
            // to the word 'descending' which is used by AutoService.
            sort = (!string.IsNullOrWhiteSpace(sort) && sort.Contains('-')) ? sort.TrimStart("-".ToCharArray()) + " descending" : sort;

            _deductionService.ProxyCreationEnabled = false;

            _deductionService.LazyLoadingEnabled = false;

            var result = _deductionService.Get(skip: skip, take: perPage, includeProperties: expand, filterString: "payment.contract.contractNumber == \"" + contractNumber + "\"", orderByString: sort);

            return Ok(result);
        }

        [Route("getDeductionsByContractNumberCount")]
        [HttpGet]
        public IHttpActionResult GetDeductionsByContractNumberCount(string contractNumber, string sort = null, string expand = null, string fields = null, string q = null, int page = 1, int perPage = Int32.MaxValue)
        {
            var result = _deductionService.GetCount(filterString: "payment.contract.contractNumber == \"" + contractNumber + "\"");

            return Ok(result);
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
