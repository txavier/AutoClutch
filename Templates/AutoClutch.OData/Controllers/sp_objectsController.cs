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
    [RoutePrefix("odata")]
    public class sp_objectsController : ODataApiController<sp_object>
    {
        public Isp_objectService _sp_objectService { get; set; }
        public ILogService<sp_object> _logService { get; set; }

        public sp_objectsController(Isp_objectService sp_objectService, ILogService<sp_object> logService)
            : base(sp_objectService, logService)
        {
            _sp_objectService = sp_objectService;
            _logService = logService;
        }

        private bool EntityExists(String key)
        {
            var result = _sp_objectService.Exists(key);

            return result;
        }

        [HttpGet]
        [EnableQuery]
        [ODataRoute("getByKeyString(Key={Key})")]
        public IHttpActionResult getByKeyString([FromODataUri] String Key)
        {
            var result = _sp_objectService.Queryable().Where(i => i.Sspl_Code == Key).ToList();
            return Ok(result);


        }
       /* [HttpGet]
        [ODataRoute("GetTotal(filter={filter})")]
        public String[] GetTotal([FromODataUri] String filter)
        {
            var result = _sp_objectService.GetTotal(filter);

            return result;
        }*/

        [HttpPut]
        [ODataRoute("sp_objects/Default.putByKeyString")]
        public async Task<IHttpActionResult> putByKeyString(ODataActionParameters parameters)
        {
            try
            {
                sp_object entity = (sp_object)parameters["entity"];

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    entity = await _sp_objectService.UpdateAsync(entity, User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault());

                    if (_sp_objectService.Errors.Any())
                    {
                        return RetrieveErrorResult(_sp_objectService.Errors);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return Updated(entity);
            }
            catch (Exception ex)
            {
                _logService?.Info(ex);
                return InternalServerError(ex);
            }
        }
    }
}
