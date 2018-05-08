using AutoClutch.Core.Interfaces;
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
    public class SectionsODataController : ODataApiController<section>
    {
        private IService<section> _sectionService;

        public SectionsODataController(IService<section> sectionService)
            : base(sectionService)
        {
            _sectionService = sectionService;
        }

    }
}
