namespace $safeprojectname$.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Core.Models;
    using TrackerEnabledDbContext.Common.Extensions;
    using AutoClutch.Auto.Core.Objects;

    public partial class EfDataDbContext : TrackerEnabledDbContext.TrackerContext
    {
        public EfDataDbContext()
            : base("name=EfDataDbContext")
        {
        }

        public virtual DbSet<user> users { get; set; }

        public virtual DbSet<userActionLog> userActionLog { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            TrackingConfig.SetTrackingProperties(modelBuilder);
        }
    }
}
