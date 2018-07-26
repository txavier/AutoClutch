namespace $safeprojectname$.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using AutoClutchTemplate.Core.Models;

    public partial class EfDataDbContext : TrackerEnabledDbContext.TrackerContext
    {
        public EfDataDbContext()
            : base("name=EfDataDbContext")
        {
            //public virtual DbSet<user> users { get; set; }
        }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            TrackingConfig.SetTrackingProperties(modelBuilder);
        }
    }
}
