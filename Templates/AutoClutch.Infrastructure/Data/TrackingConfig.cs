using AutoClutch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerEnabledDbContext.Common.Extensions;

namespace $safeprojectname$
{
    public class TrackingConfig
    {
        public static void SetTrackingProperties(DbModelBuilder modelBuilder)
        {
            //TrackerEnabledDbContext.Common.Configuration.GlobalTrackingConfig.SetSoftDeletableCriteria<ISoftDeletable>(entity => entity.IsDeleted);

            modelBuilder.Entity<user>().TrackAllProperties();

            modelBuilder.Entity<setting>().TrackAllProperties();

            modelBuilder.Entity<blogEntry>().TrackAllProperties();
        }
    }
}
