using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
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
    public class ProjectsController : ODataApiController<project>
    {
        private IService<project> _projectService;

        public ProjectsController(IService<project> projectService, ILogService<project> logService)
            : base(projectService, logService)
        {
            _projectService = projectService;
        }
    }
}
