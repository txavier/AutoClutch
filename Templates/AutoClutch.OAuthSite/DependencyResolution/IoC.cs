using WebX.CompositionRoot;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c => { c.AddRegistry<DefaultRegistry>(); });
        }
    }
}