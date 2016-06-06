namespace $safeprojectname$.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("engineer")]
    public partial class engineer
    {
        internal bool? adminRole;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public engineer()
        {
            //workOrders = new HashSet<workOrder>();
        }

        public int engineerId { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(50)]
        public string phoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string emailAddress { get; set; }

        public int? sectionId { get; set; }

        public string userName { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<workOrder> workOrders { get; set; }
    }
}
