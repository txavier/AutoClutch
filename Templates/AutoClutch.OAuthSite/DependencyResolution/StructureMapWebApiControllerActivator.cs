using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace $safeprojectname$.DependencyResolution
{
    public class StructureMapWebApiControllerActivator : IHttpControllerActivator
    {
        private readonly IContainer _container;

        public StructureMapWebApiControllerActivator(IContainer container)
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            // Allow the default and select .net controllers to use the default dependency
            // injection.
            if(controllerDescriptor.ControllerName == "Me")
            {
                return new Controllers.MeController() as IHttpController;
            }       

            var nested = _container.GetNestedContainer();
            var instance = nested.GetInstance(controllerType) as IHttpController;
            request.RegisterForDispose(nested);

            return instance;
        }
    }
}