using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface ISqlServerReportGetter
    {
        string Extension { get; set; }

        string Encoding { get; set; }

        string MimeType { get; set; }

        IEnumerable<string> StreamIDs { get; set; }

        byte[] GenerateReport(string serverName, string reportPath, string format = null, string fileName = null, List<KeyValuePair<string, string>> parameterDictionary = null, string historyID = null);
    }
}
