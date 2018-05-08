using AutoClutch.Controller;
using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    public class EngineersODataController : ODataApiController<engineer>
    {
        
        private IContractDocumentService _contractDocumentService;
        private IService<engineer> _engineerService;

        private ILogService<engineer> _logService;
        public EngineersODataController(IService<engineer> engineerService, ILogService<engineer> logService, IContractDocumentService contractDocumentService)
            : base(engineerService, logService)
        {
            _engineerService = engineerService;

            _logService = logService;

            _contractDocumentService = contractDocumentService;
        }
        [Route("getEngineers")]
        [HttpGet]
        public IHttpActionResult GetEngineers(string sectionName)
        {
            _engineerService.LazyLoadingEnabled = false;

            _engineerService.ProxyCreationEnabled = false;

            var result = _engineerService.Queryable().Where(i => i.section.name == sectionName);

            return Ok(result);
        }

        [Route("getLoggedInUser")]
        [HttpGet]
        public IHttpActionResult GetLoggedInUser()
        {
            _engineerService.LazyLoadingEnabled = false;

            _engineerService.ProxyCreationEnabled = false;

            var userName = User.Identity.Name.Split('\\').LastOrDefault();

            var user = _engineerService.Queryable().SingleOrDefault(i => i.userName.Equals(userName));

            return Ok(user);
        }

        [Route("MigrateFiles")]
        [HttpGet]
        public IHttpActionResult MigrateFiles()
        {
            _contractDocumentService.DmsMigrateFiles();

            return Ok();
        }
    }
}
