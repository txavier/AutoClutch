using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface IReportService
    {
        string Extension { get; set; }

        string Encoding { get; set; }

        string MimeType { get; set; }

        IEnumerable<string> StreamIDs { get; set; }

        byte[] GetReport(string reportServerName, string reportName, string format, List<KeyValuePair<string, string>> parameters);
    }
}
