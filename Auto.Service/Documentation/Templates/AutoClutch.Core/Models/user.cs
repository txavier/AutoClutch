namespace $safeprojectname$.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
        }

        public int userId { get; set; }

        [Required]
        [StringLength(255)]
        public string firstName { get; set; }

        [Required]
        [StringLength(255)]
        public string lastName { get; set; }

        [StringLength(50)]
        public string phoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string emailAddress { get; set; }

        public string userName { get; set; }
    }
}
