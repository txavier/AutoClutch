using AutoClutch.Repo.Interfaces;
using StructureMap.Configuration.DSL;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClutch.Core;
using AutoClutch.Core.Interfaces;
using AutoClutch.Repo;
using AutoClutch.Log.Services;

namespace $safeprojectname$
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssembliesFromApplicationBaseDirectory();
                scan.WithDefaultConventions();
            });

            For<DbContext>().Use<EfDataDbContext>();

            For(typeof(IService<>)).Use(typeof(Service<>));

            For(typeof(IRepository<>)).Use(typeof(Repository<>));

            For<IEnvironmentConfigSettingsGetter>().Use<EnvironmentConfigSettingsGetter>();

            For(typeof(AutoClutch.Core.Interfaces.ILogService<>)).Use(typeof(UserActionLogService<>));
        }
    }
}
