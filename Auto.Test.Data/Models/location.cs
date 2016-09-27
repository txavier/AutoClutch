namespace AutoClutch.Test.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("location")]
    public partial class location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public location()
        {
            facilities = new HashSet<facility>();
        }

        public int locationId { get; set; }

        public int? contactUserId { get; set; }

        [Required]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<facility> facilities { get; set; }

        public virtual user user { get; set; }
    }
}
