using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/files")]
    public class FilesController : ApiController
    {
        private IContractDocumentService _contractDocumentService;

        public FilesController(IContractDocumentService contractDocumentService)
        {
            _contractDocumentService = contractDocumentService;
        }

        /// <summary>
        /// This method returns the byte array of an uploaded file.
        /// https://github.com/danialfarid/ng-file-upload
        /// https://github.com/stewartm83/angular-fileupload-sample
        /// https://code.msdn.microsoft.com/AngularJS-with-Web-API-22f62a6e
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                return BadRequest("Unsupported media type");
            }
            try
            {
                // Read in the file and return it as a byte array for use in putting
                // the byte array into a model possibly bound for the database.
                var body = await Request.Content.ReadAsByteArrayAsync();

                // If this is a very large file then we will only send
                // back the file id and not the full content of the file.
                if (Request.Content.Headers.ContentLength > 200000000)
                {
                    // Read the file and form data.
                    var depFile = _contractDocumentService.Add(body, "tempFile");

                    if(depFile == null)
                    {
                        // Get any errors.
                        if (_contractDocumentService.Errors.Any())
                        {
                            return RetrieveErrorResult(_contractDocumentService.Errors);
                        }
                    }

                    return Ok(depFile.fileId);
                }

                return Ok(body);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetBaseException().Message);
            }
        }

        protected IHttpActionResult RetrieveErrorResult(IEnumerable<AutoClutch.Core.Objects.Error> errors)
        {
            if (errors != null && errors.Any())
            {
                foreach (var error in errors.Select(i => i.Description).Distinct())
                {
                    ModelState.AddModelError("", error);
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, 
                    // so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }
            return null;
        }

        public HttpResponseMessage Get(string address, int? fileId = null)
        {
            HttpResponseMessage result = null;

            // If there is a file id then we assume that the file is stored in the 
            // DEP Document management system and we try to retrieve it from there.
            if (fileId.HasValue)
            {
                result = Request.CreateResponse(HttpStatusCode.OK);

                var depFile = _contractDocumentService.GetFile(fileId.Value);

                var byteArray = depFile.fileStream;

                result.Content = new ByteArrayContent(byteArray);
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition.FileName = depFile.fileName;
                result.Content.Headers.Add("x-filename", depFile.fileName);
            }
            else if (address != null)
            {   
                // Serve the file to the client.
                result = Request.CreateResponse(HttpStatusCode.OK);
                result.Content = new StreamContent(new System.IO.FileStream(address, System.IO.FileMode.Open, System.IO.FileAccess.Read));
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition.FileName = address.Split("\\".ToCharArray()).LastOrDefault() ?? "Test.pdf";
                result.Content.Headers.Add("x-filename", address.Split("\\".ToCharArray()).LastOrDefault() ?? "Test.pdf");
            }
            else 
            {
                result = Request.CreateResponse(HttpStatusCode.Gone);
            }

            return result;
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _contractDocumentService.Remove(id);

            return Ok();
        }

    }
}
