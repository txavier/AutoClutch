using AutoClutch.Repo.Interfaces;
using StructureMap;
using System.Data.Entity;
using AutoClutch.Core;
using AutoClutch.Core.Interfaces;
using AutoClutch.Repo;
using AutoClutch.Log.Services;
using AutoClutchTemplate.Core.Interfaces;
using AutoClutchTemplate.Infrastructure.Data;

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

            //For<IEnvironmentConfigSettingsGetter>().Use<EnvironmentConfigSettingsGetter>();

            For(typeof(AutoClutch.Core.Interfaces.ILogService<>)).Use(typeof(UserActionLogService<>));
        }
    }
}
