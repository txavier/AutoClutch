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
    public class ChangeOrderTypesController : BaseApiController<changeOrderType>
    {
        private IService<changeOrderType> _changeOrderService;

        public ChangeOrderTypesController(IService<changeOrderType> changeOrderService)
            : base(changeOrderService)
        {
            _changeOrderService = changeOrderService;
        }

    }
}
