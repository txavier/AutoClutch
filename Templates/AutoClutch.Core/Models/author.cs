using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Models
{
    [Table("author")]
    public partial class author
    {
        public author()
        {
            blogEntries = new HashSet<blogEntry>();
        }

        public int authorId { get; set; }

        public string authorFirstName { get; set; }

        public string authorLastName { get; set; }

        public string authorUsername { get; set; }

        public string aboutTheAuthorHtml { get; set; }

        public virtual ICollection<blogEntry> blogEntries { get; set; }
    }
}