using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Models
{
    public partial class blogEntry
    {
        //[NotMapped]
        //public string monthAbbreviation
        //{
        //    get { return GetMonthAbbreviation(this.publishedDate); }
        //    set { }
        //}

        //[NotMapped]
        //public int day { get { return this.publishedDate.Day; } set { } }

        //[NotMapped]
        //public int year { get { return this.publishedDate.Year; } set { } }

        //[NotMapped]
        //public string blogBodySummaryHtml
        //{
        //    get { return GetBodySummaryHtml(); }
        //    set { }
        //}

        //public string GetMonthAbbreviation(DateTime publishedDate)
        //{
        //    var result = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(publishedDate.Month);

        //    return result;
        //}

        //private string GetBodySummaryHtml()
        //{
        //    var result = blogBodyHtml.Length > 839 ? blogBodyHtml.Substring(0, (int)Math.Round((double)blogBodyHtml.Length / 4)) + "..." :
        //        blogBodyHtml;

        //    return result;
        //}
    }
}
