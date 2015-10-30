namespace Auto.Test.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("facility")]
    public partial class facility
    {
        public int facilityId { get; set; }

        public int? locationId { get; set; }

        [Required]
        public string name { get; set; }

        public string facilityType { get; set; }

        public virtual location location { get; set; }
    }
}
