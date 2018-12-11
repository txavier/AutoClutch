using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/allocations")]
    public class AllocationsController : ApiController
    {
        private IAllocationsService _allocationsService;

        public AllocationsController(IAllocationsService allocationsService)
        {
            _allocationsService = allocationsService;
        }

        [HttpGet]
        [Route("getAllocations({type},{searchBy},{searchValue})")]
        public List<AllocationsData> getAllocations(string type, string searchBy, string searchValue)
        {
            var result = _allocationsService.getAllocations(type, searchBy, searchValue);

            return result;
        }

        [HttpGet]
        [Route("getAllAllocations({type},{searchBy},{searchValue})")]
        public List<AllocationsData> getAllAllocations()
        {
            var result = _allocationsService.getAllocations(null, null, "%");

            return result;
        }
    }
}