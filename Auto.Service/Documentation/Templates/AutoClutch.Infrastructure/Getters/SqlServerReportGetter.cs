using NerdLunch.Core.Interfaces;
using $safeprojectname$.ReportExecutionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Getters
{
    // http://stackoverflow.com/questions/12307931/generate-reports-programmatically-using-the-tfs-api-and-ssrs
    // https://msdn.microsoft.com/en-US/library/microsoft.wssux.reportingserviceswebservice.rsexecutionservice2005.reportexecutionservice.render(v=SQL.90).aspx
    public class SqlServerReportGetter : ISqlServerReportGetter
    {
        public string Extension { get; set; }

        public string Encoding { get; set; }

        public string MimeType { get; set; }

        public IEnumerable<string> StreamIDs { get; set; }

        /// <summary>
        /// Returns a report from the SSRS server.
        /// </summary>
        /// <param name="serverName">Name of the report server. i.e. lfkbwtlabsql01</param>
        /// <param name="reportPath">Location of the report path. i.e. '/CRT/Reports/ContainerStatusReportByDate'</param>
        /// <param name="format">The format of the file. i.e. XML, NULL, CSV, IMAGE, PDF, HTML4.0, HTML3.2, MHTML, EXCEL, and HTMLOWC.  The default is PDF.</param>
        /// <param name="fileName">Filename with extension. i.e. "\\server\c$\report.pdf"</param>
        /// <param name="parameterDictionary">Report parameters i.e. Key = "DateEnter", Value = "2015-10-11 00:00:00" or in JSON { DateEnter: '1/1/2016', Plant: 'Newtown Creek' }
        /// </param>
        public byte[] GenerateReport(string serverName, string reportPath, string format = null, string fileName = null, List<KeyValuePair<string, string>> parameterDictionary = null, string historyID = null)
        {
            byte[] result = null;

            using (ReportExecutionService.ReportExecutionService rs = new ReportExecutionService.ReportExecutionService())
            {
                // Web service option setup.
                rs.Timeout = 300000;

                rs.Credentials = System.Net.CredentialCache.DefaultCredentials;

                rs.Url = "http://" + serverName + "/ReportServer/ReportExecution2005.asmx";

                // Prepare report parameter.
                DataSourceCredentials[] credentials = null;

                string showHideToggle = null;

                string encoding;

                string mimeType;

                string extension;

                Warning[] warnings = null;

                ParameterValue[] reportHistoryParameters = null;

                string[] streamIDs = null;

                rs.ExecutionHeaderValue = new ExecutionHeader();

                ExecutionInfo execInfo = rs.LoadReport(reportPath, historyID);

                var parameters_ = rs.GetExecutionInfo().Parameters;

                ParameterValue[] executionParameters = GetExecutionParameterArray(parameterDictionary);

                rs.SetExecutionParameters(executionParameters, "en-us");

                String SessionId = rs.ExecutionHeaderValue.ExecutionID;

                Console.WriteLine("SessionID: {0}", rs.ExecutionHeaderValue.ExecutionID);

                try
                {
                    string devInfo = @"<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";

                    result = rs.Render(format, devInfo, out extension, out encoding, out mimeType, out warnings, out streamIDs);

                    Extension = extension;

                    Encoding = encoding;

                    MimeType = mimeType;

                    StreamIDs = streamIDs;

                    execInfo = rs.GetExecutionInfo();

                    Console.WriteLine("Execution date and time: {0}", execInfo.ExecutionDateTime);

                }
                catch (System.Web.Services.Protocols.SoapException e)
                {
                    Console.WriteLine(e.Detail.OuterXml);
                }
                // Write the contents of the report to an MHTML file.
                try
                {
                    if (fileName != null)
                    {
                        System.IO.FileStream stream = System.IO.File.Create(fileName, result.Length);

                        Console.WriteLine("File created.");

                        stream.Write(result, 0, result.Length);

                        Console.WriteLine("Result written to the file.");

                        stream.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return result;
        }

        private static ParameterValue[] GetExecutionParameterArray(List<KeyValuePair<string, string>> parameterDictionary)
        {
            var parameterValues = new List<ParameterValue>();

            if (parameterDictionary != null)
            {
                foreach (var parameter in parameterDictionary)
                {
                    var parameterValue = new ParameterValue() { Name = parameter.Key, Value = parameter.Value };

                    parameterValues.Add(parameterValue);
                }
            }

            var parameters = parameterValues.ToArray();
            return parameters;
        }
    }
}
