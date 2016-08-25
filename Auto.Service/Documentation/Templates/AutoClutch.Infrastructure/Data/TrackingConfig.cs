using NerdLunch.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerEnabledDbContext.Common.Extensions;

namespace $safeprojectname$.Data
{
    public class TrackingConfig
    {
        public static void SetTrackingProperties(DbModelBuilder modelBuilder)
        {
            //TrackerEnabledDbContext.Common.Configuration.GlobalTrackingConfig.SetSoftDeletableCriteria<ISoftDeletable>(entity => entity.IsDeleted);

            modelBuilder.Entity<user>().TrackAllProperties();

            modelBuilder.Entity<setting>().TrackAllProperties();

            modelBuilder.Entity<lunch>().TrackAllProperties();

            modelBuilder.Entity<restaurantLocation>().TrackAllProperties();

            modelBuilder.Entity<lunchInvite>().TrackAllProperties();
        }
    }
}
