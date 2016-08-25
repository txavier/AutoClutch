using $safeprojectname$.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    public class ReportService : IReportService
    {
        private readonly ISqlServerReportGetter _sqlServerReportGetter;

        public string Extension { get; set; }

        public string Encoding { get; set; }

        public string MimeType { get; set; }

        public IEnumerable<string> StreamIDs { get; set; }

        public ReportService(ISqlServerReportGetter sqlServerReportGetter)
        {
            _sqlServerReportGetter = sqlServerReportGetter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportServerName"></param>
        /// <param name="reportPath"></param>
        /// <param name="format"></param>
        /// <param name="parameters">The report parameters (i.e. { DateEnter: '1/1/2016', Plant: 'Newtown Creek' })</param>
        /// <returns></returns>
        public byte[] GetReport(string reportServerName, string reportPath, string format, List<KeyValuePair<string, string>> parameters)
        {
            var result = _sqlServerReportGetter.GenerateReport(reportServerName, reportPath, format, parameterDictionary: parameters);

            // Fill properties.
            Extension = _sqlServerReportGetter.Extension;

            Encoding = _sqlServerReportGetter.Encoding;

            MimeType = _sqlServerReportGetter.MimeType;

            StreamIDs = _sqlServerReportGetter.StreamIDs;

            return result;
        }
    }
}
