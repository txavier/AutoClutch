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
    public class FileGroupsController : ODataApiController<fileGroup>
    {
        private IService<fileGroup> _fileGroupService;

        public FileGroupsController(IService<fileGroup> fileGroupService, ILogService<fileGroup> logService)
            : base(fileGroupService, logService)
        {
            _fileGroupService = fileGroupService;
        }
    }
}
