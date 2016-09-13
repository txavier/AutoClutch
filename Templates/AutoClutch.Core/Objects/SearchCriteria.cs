using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Objects
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            searchParams = new List<SearchParam>();
        }

        public int? currentPage { get; set; }

        public int? itemsPerPage { get; set; }

        public string orderBy { get; set; }

        public string searchText { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Params")]
        public IEnumerable<SearchParam> searchParams { get; set; }

        public string includeProperties { get; set; }

        public DateTime? startDateTime { get; set; }

        public DateTime? endDateTime { get; set; }
    }
}
