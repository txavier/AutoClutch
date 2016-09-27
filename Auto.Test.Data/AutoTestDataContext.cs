namespace AutoClutch.Test.Data
{
    using AutoClutch.Core.Interfaces;
    using System.Data.Entity;
    using TrackerEnabledDbContext.Common.Extensions;

    public partial class AutoTestDataContext : TrackerEnabledDbContext.TrackerContext
    {
        public AutoTestDataContext()
            : base("name=AutoTestDataContext")
        {
        }

        public virtual DbSet<facility> facilities { get; set; }
        public virtual DbSet<location> locations { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DatabaseGeneratedAttribute

            var userEntity = modelBuilder.Entity<user>();

            userEntity.TrackAllProperties();

            userEntity
                .HasMany(e => e.locations)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.contactUserId);

            TrackerEnabledDbContext.Common.Configuration.GlobalTrackingConfig.SetSoftDeletableCriteria<ISoftDeletable>(entity => entity.IsDeleted);

            modelBuilder.Entity<location>().TrackAllProperties();

            modelBuilder.Entity<facility>().TrackAllProperties();
        }
    }
}
