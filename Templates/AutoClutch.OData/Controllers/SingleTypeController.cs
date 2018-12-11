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
    [RoutePrefix("api/singletype")]
    public class SingleTypeController : ApiController
    {
        private ISingleTypeService _singleTypeService;

        public SingleTypeController(ISingleTypeService singleTypeService)
        {
            _singleTypeService = singleTypeService;
        }

        [HttpGet]
        [Route("getSingleTypeDataBatch({Rep_Cat},{Alloc_Type})")]
        public SingleTypeDataBatch GetSingleTypeData(string Rep_Cat, string Alloc_Type)
        {
            var result = _singleTypeService.GetSingleTypeDataBatch(Rep_Cat, Alloc_Type);

            return result;
        }

        [HttpGet]
        [Route("getSingleTypeContractBatch({Rep_Cat},{Alloc_Type})")]
        public SingleTypeContractBatch GetSingleTypeContract(string Rep_Cat, string Alloc_Type)
        {
            var result = _singleTypeService.GetSingleTypeContractBatch(Rep_Cat, Alloc_Type);

            return result;
        }
    }
}