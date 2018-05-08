using System;
using System.Threading.Tasks;
using System.Web;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using Hangfire;
using Hangfire.StructureMap;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof($safeprojectname$.Startup))]

namespace $safeprojectname$
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            ConfigureHangfire(app);
        }

        private static void ConfigureHangfire(IAppBuilder app)
        {
            var container = IoC.Initialize();

            GlobalConfiguration.Configuration.UseActivator(new StructureMapJobActivator(container));

            GlobalConfiguration.Configuration.UseSqlServerStorage("EfDataDbContext");

            app.UseHangfireServer();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                AppPath = VirtualPathUtility.ToAbsolute("~/"),
                AuthorizationFilters = new[] { new HangfireConfiguration.HangfireAuthorizationFilter() }
            });

#if !DEBUG
            var timelineItemService = container.GetInstance<ITimelineItemService>();

            RecurringJob.AddOrUpdate("sendOverdueTimelineItemEmails", () => timelineItemService.EmailOverdueThreadItemPointPersons(), Cron.Weekly(DayOfWeek.Monday, 9));

            //// This is the hourly job to make sure the mvo dispatchers are properly returning and dispatching jobs in a timely fashion.
            //var sessionService = container.GetInstance<ISessionService>();

            //// The below line was commented out because it gets changed by the line after it and nobody seemed to notice it wasnt
            //// happening :)
            ////RecurringJob.AddOrUpdate(() => sessionService.SendMvoNotificationSessionsEmailMessage(), Cron.Daily(7));

            //RecurringJob.AddOrUpdate(() => sessionService.SendMvoNotificationSessionsEmailMessage(), Cron.Daily(19));

            //// This is the daily job to send a notification to plants that have not submitted their container status updates for the day.
            //var containerRemovalTrackingEmailService = container.GetInstance<IContainerRemovalTrackingEmailService>();

            //RecurringJob.AddOrUpdate(() => containerRemovalTrackingEmailService.EmailPlantsThatHaveNotSubmittedFailed(), Cron.Daily(16));

            //RecurringJob.RemoveIfExists("SessionService.SendStateOfContainersEmailMessage");

            //RecurringJob.AddOrUpdate("morningStateOfContainers", () => sessionService.SendStateOfContainersEmailMessage(), Cron.Daily(8));

            //RecurringJob.AddOrUpdate("afternoonStateOfContainers", () => sessionService.SendStateOfContainersEmailMessage(), Cron.Daily(20));
#endif
        }
    }
}
