namespace AutoClutch.Test.Data
{
    using AutoClutch.Core.Interfaces;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("facility")]
    public partial class facility : ISoftDeletable
    {
        public int facilityId { get; set; }

        public int? locationId { get; set; }

        [Required]
        public string name { get; set; }

        public string facilityType { get; set; }

        public virtual location location { get; set; }

        public bool IsDeleted { get; set; }
    }
}
