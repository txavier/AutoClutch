namespace Auto.Test.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AutoTestDataContext : DbContext
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

            modelBuilder.Entity<user>()
                .HasMany(e => e.locations)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.contactUserId);
        }
    }
}
