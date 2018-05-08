using AutoClutch.Controller;
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
    public class ChangeOrderTypesODataController : ODataApiController<changeOrderType>
    {
        private IService<changeOrderType> _changeOrderService;

        public ChangeOrderTypesODataController(IService<changeOrderType> changeOrderService)
            : base(changeOrderService)
        {
            _changeOrderService = changeOrderService;
        }

    }
}
