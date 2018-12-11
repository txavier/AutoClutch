using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
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
    public class TracksController : ODataApiController<Track>
    {
        public IService<Track> _TrackService { get; set; }

        public TracksController(IService<Track> TrackService, ILogService<Track> logService)
            : base(TrackService, logService)
        {
            _TrackService = TrackService;
        }
    }
}
