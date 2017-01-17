using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Models
{
    [Table("blogEntry")]
    public partial class blogEntry
    {
        public blogEntry()
        {
            blogTags = new HashSet<blogTag>();

            blogTopics = new HashSet<blogCategory>();
        }

        public int blogEntryId { get; set; }

        public string title { get; set; }

        public int authorId { get; set; }

        public author author { get; set; }

        public virtual ICollection<blogTag> blogTags { get; set; }

        public virtual ICollection<blogCategory> blogTopics { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime publishedDate { get; set; }

        public string imageBase64String { get; set; }

        public string blogBodyHtml { get; set; }
    }
}
