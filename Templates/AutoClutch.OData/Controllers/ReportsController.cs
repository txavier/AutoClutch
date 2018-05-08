using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    public class ReportsController
    {
        public class reportsController : ApiController
        {
            private ICTMReportService _reportService;

            public reportsController()
            {
                var container = IoC.Initialize();

                _reportService = container.GetInstance<ICTMReportService>();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="reportServerName"></param>
            /// <param name="reportPath"></param>
            /// <param name="format"></param>
            /// <param name="parameters">The report parameters (i.e. { DateEnter: '1/1/2016', Plant: 'Newtown Creek' })</param>
            /// <returns></returns>
            [HttpPost]
            public HttpResponseMessage Post(string reportName, string format, List<KeyValuePair<string, string>> parameters)
            {
                HttpResponseMessage result = null;

                var byteArray = _reportService.GetReport(reportName, format, parameters);

                if (format.ToLower() == "pdf")
                {
                    // serve the file to the client
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new ByteArrayContent(byteArray);
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                    result.Content.Headers.ContentDisposition.FileName = "report.pdf";
                    result.Content.Headers.Add("x-filename", "report.pdf");
                }
                else if (format.ToLower().Contains("html"))
                {
                    string s = Encoding.ASCII.GetString(byteArray);

                    result = new HttpResponseMessage();

                    result.Content = new StringContent(s);
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                }
                else if (format.ToLower().Contains("csv"))
                {
                    string s = Encoding.ASCII.GetString(byteArray);

                    result = new HttpResponseMessage();

                    result.Content = new StringContent(s);
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                }
                else if (format.ToLower().Contains("xml"))
                {
                    //string s = Encoding.ASCII.GetString(byteArray);

                    //result = new HttpResponseMessage();

                    //result.Content = new StringContent(s);
                    //result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
                    result = new HttpResponseMessage();

                    var ms = new MemoryStream(byteArray);

                    result.Content = new StreamContent(ms);
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_reportService.Encoding);
                }
                else if (format.ToLower().Contains("excel"))
                {
                    result = new HttpResponseMessage();

                    var ms = new MemoryStream(byteArray);

                    result.Content = new StreamContent(ms);
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(_reportService.Encoding);
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = "report." + _reportService.Extension;
                }

                return result;
            }

        }
    }
}