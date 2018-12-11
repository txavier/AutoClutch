using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OTPS.Core.Interfaces;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/alldata")]
    public class AllDataController : ApiController
    {
        private IAllDataService _allDataService;

        public AllDataController(IAllDataService allDataService)
        {
			_allDataService = allDataService;
        }

        [HttpGet]
        [Route("getAllDataBatch({Rep_Cat})")]
        public AllDataBatch GetAllDataBatch(string Rep_Cat)
        {
            var result = _allDataService.GetAllDataBatch(Rep_Cat);

            return result;
        }

        [HttpGet]
        [Route("getAllDataContract({Rep_Cat})")]
        public AllDataContractBatch GetAllDataContract(string Rep_Cat)
        {
            var result = _allDataService.GetAllDataContractBatch(Rep_Cat);

            return result;
        }
    }
}