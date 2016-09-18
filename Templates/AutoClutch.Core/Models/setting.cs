namespace $safeprojectname$.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("setting")]
    public partial class setting
    {
        public int settingId { get; set; }

        [Required]
        public string settingKey { get; set; }

        public string settingValue { get; set; }
    }
}
