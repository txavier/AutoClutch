using AutoClutch.Core.Interfaces;
using AutoClutch.Core.Interfaces;
using OTPS.Core.Models;
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
        private IService<setting> _settingService;

        public SettingsController(IService<setting> settingService)
            : base(settingService)
        {
            _settingService = settingService;
        }

    }
}
