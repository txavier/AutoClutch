namespace WebX.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Core.Models;
    using TrackerEnabledDbContext.Common.Extensions;

    public partial class EfDataDbContext : TrackerEnabledDbContext.TrackerContext
    {
        public EfDataDbContext()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<user> users { get; set; }

        public virtual DbSet<AutoClutch.Core.Models.userActionLog> userActionLogs { get; set; }

        public virtual DbSet<setting> settings { get; set; }

        public virtual DbSet<author> authors { get; set; }

        public virtual DbSet<blogEntry> blogEntries { get; set; }

        public virtual DbSet<blogTag> blogTags { get; set; }

        public virtual DbSet<blogCategory> blogCategories { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            TrackingConfig.SetTrackingProperties(modelBuilder);
        }
    }
}
