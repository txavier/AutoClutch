using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Interfaces;
using OTPS.Core.Models;
using OTPS.Core.Objects;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
    [RoutePrefix("api/GetTotal")]
    public class GetTotalController : ApiController
    {
        public IGetTotalService _getTotalService;
        public IGetTotalService _getLineTotalService;


        public GetTotalController(IGetTotalService getTotalService, IGetTotalService getLineTotalService)//
        {
            _getTotalService = getTotalService;
            _getLineTotalService = getLineTotalService;
        }


        [HttpGet]
        [Route("GetTotal(filter={filter})")]
        public String[] GetTotal([FromUri] String filter)
        {
            var result = _getTotalService.GetTotal(filter);

            return result;
        }

        [HttpGet]
        [Route("GetLineTotal(filter={filter})")]
        public String[] GetLineTotal([FromUri] String filter)
        {
            var result = _getLineTotalService.GetLineTotal(filter);

            return result;
        }
    }
}
