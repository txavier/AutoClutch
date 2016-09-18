using AutoClutch.Auto.Repo.Interfaces;
using AutoClutch.Auto.Repo.Objects;
using AutoClutch.Auto.Service.Interfaces;
using AutoClutch.Auto.Service.Services;
using StructureMap.Configuration.DSL;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            For(typeof(ILogService<>)).Use(typeof(UserActionLogService<>));
        }
    }
}
