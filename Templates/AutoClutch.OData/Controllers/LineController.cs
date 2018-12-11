using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OTPS.Core.Interfaces;
using OTPS.Core.Models;
using OTPS.Core.Objects;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/Line")]
    public class LineController : ApiController
    {

        ILineService _LineAddService;
        ILineService _LineUpdateService;
        ILineService _LineDeleteService;
    


        public LineController(ILineService LineAddService,ILineService LineUpdateService, ILineService LineDeleteService)
        {
            _LineAddService = LineAddService;
            _LineUpdateService = LineUpdateService;
            _LineDeleteService = LineDeleteService;
        }


        [HttpPost]
        [Route("LineAdd")]
        public IHttpActionResult LineAdd(sp_line entity)
        {
            _LineAddService.LineAdd(entity);

            return Ok();
        }

        [HttpPut]
        [Route("LineUpdate({key})")]
        public IHttpActionResult LineUpdate(string key, sp_line entity) {

            _LineUpdateService.LineUpdate(key, entity);

            return Ok();
        }

        [HttpDelete]
        [Route("LineDelete(Key={Key})")]
        public void LineDelete([FromUri] string Key)
        {
            var loggedInUserName = User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault();

            _LineDeleteService.LineDelete(Key, loggedInUserName);
        }
    }
}
