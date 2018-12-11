using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Interfaces;
using OTPS.Core.Models;
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
    public class Modi_LocsController : ODataApiController<Modi_Loc>
    {
        public IModi_LocService _Modi_LocService { get; set; }
        public ILogService<Modi_Loc> _logService { get; set; }

        public Modi_LocsController(IModi_LocService Modi_LocService, ILogService<Modi_Loc> logService)
            : base(Modi_LocService, logService)
        {
            _Modi_LocService = Modi_LocService;
            _logService = logService;
        }

        // DOES NOT WORK..
        [HttpPost]
        [ODataRoute("Modi_Locs/Default.ModifyLoc")]
        public async Task<IHttpActionResult> ModifyLoc(ODataActionParameters parameters)
        {
            try
            {
                Modi_Loc entity = (Modi_Loc)parameters["entity"];

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    entity = await _Modi_LocService.AddAsync(entity, User.Identity.Name?.Split("\\".ToCharArray()).LastOrDefault());


                    if (_Modi_LocService.Errors.Any())
                    {
                        return RetrieveErrorResult(_Modi_LocService.Errors);
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
