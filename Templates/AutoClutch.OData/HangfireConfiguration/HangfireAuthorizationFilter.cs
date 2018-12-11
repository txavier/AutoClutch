using AutoClutch.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using Hangfire.Dashboard;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace $safeprojectname$.HangfireConfiguration
{
    internal class HangfireAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            // In case you need an OWIN context, use the next line.
            var context = new OwinContext(owinEnvironment);

            var container = IoC.Initialize();

            //var _userService = container.GetInstance<IService<Core.Models.user>>();

            var userName = context.Authentication.User.Identity.Name.Split('\\').LastOrDefault();

            //var user = _userService.Queryable().Where(i => i.userName == userName).FirstOrDefault();

            //if (user != null && user.authorizationPriorityLevel == 1)
            //{
            //    return true;
            //}

            return true;
        }
    }
}