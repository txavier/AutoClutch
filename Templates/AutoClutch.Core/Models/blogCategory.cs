using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace $safeprojectname$.Models
{
    [Table("blogCategory")]
    public class blogCategory
    {
        public blogCategory()
        {
        }

        public int blogCategoryId { get; set; }
        public string name { get; set; }
    }
}