using AutoClutch.Core.Interfaces;
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
    public class SettingsController : BaseApiController<setting>
    {
        private IContractDocumentService _contractDocumentService;
        private IService<setting> _settingService;

        public SettingsController(IService<setting> settingService, IContractDocumentService contractDocumentService)
            : base(settingService)
        {
            _settingService = settingService;

            _contractDocumentService = contractDocumentService;
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
