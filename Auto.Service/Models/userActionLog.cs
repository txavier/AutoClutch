using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoClutch.Core.Objects
{
    [Table("userActionLog")]
    public partial class userActionLog
    {
        [Required]
        public string body { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date { get; set; }

        public string typeFullName { get; set; }

        public int recordId { get; set; }

        public int eventType { get; set; }

        public string eventTypeDisplay { get; set; }

        public int userActionLogId { get; set; }
    }
}
