using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Net.Http;

namespace $safeprojectname$.DependencyResolution
{
    /// <summary>
    /// http://cdroulers.com/blog/2015/03/03/structuremap-3-and-asp-net-web-api-2/
    /// </summary>
    public class StructureMapWebApiControllerActivator : IHttpControllerActivator
    {
        private IContainer _container;

        public StructureMapWebApiControllerActivator(IContainer container)
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var nested = _container.GetNestedContainer();
            var instance = nested.GetInstance(controllerType) as IHttpController;
            request.RegisterForDispose(nested);

            return instance;
        }
    }
}