using AutoClutch.Core.Interfaces;
using Elmah;
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
using System.Web;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    [RoutePrefix("api/sharepointfiles")]
    public class SharepointFilesController : ApiController
    {
        private ISharepointFileService _sharepointFileService;
        private IService<file> _fileService;
        private IService<fileGroup> _fileGroupService;

        public SharepointFilesController(ISharepointFileService sharepointFileService, IService<file> fileService, IService<fileGroup> fileGroupService)
        {
            _sharepointFileService = sharepointFileService;

            _fileService = fileService;

            _fileGroupService = fileGroupService;
        }

        /// <summary>
        /// This method returns the byte array of an uploaded file.
        /// https://github.com/danialfarid/ng-file-upload
        /// https://github.com/stewartm83/angular-fileupload-sample
        /// https://code.msdn.microsoft.com/AngularJS-with-Web-API-22f62a6e
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<IHttpActionResult> Post1()
        //{
        //    // Check if the request contains multipart/form-data.
        //    if (!Request.Content.IsMimeMultipartContent("form-data"))
        //    {
        //        return BadRequest("Unsupported media type");
        //    }
        //    try
        //    {
        //        // Read in the file and return it as a byte array for use in putting
        //        // the byte array into a model possibly bound for the database.
        //        var body = await Request.Content.ReadAsByteArrayAsync();

        //        // If this is a very large file then we will only send
        //        // back the file id and not the full content of the file.
        //        if (Request.Content.Headers.ContentLength > 200000000)
        //        {
        //            // Read the file and form data.
        //            // TODO: Can we get the file name?
        //            var fileGuid = _sharepointFileService.UploadFileToSharePoint(body, "tempFile");

        //            if (fileGuid == null)
        //            {
        //                // Get any errors.
        //                if (_sharepointFileService.Errors.Any())
        //                {
        //                    return RetrieveErrorResult(_sharepointFileService.Errors);
        //                }
        //            }

        //            return Ok(fileGuid);
        //        }

        //        return Ok(body);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.GetBaseException().Message);
        //    }
        //}

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
                string root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new MultipartFormDataStreamProvider(root);
                await Request.Content.ReadAsMultipartAsync(provider);

                var databaseFiles = new List<file>();
                var fileGroup = new fileGroup();

                foreach(MultipartFileData file in provider.FileData)
                {
                    // Get the uploaded file and turn it into a byte array.
                    var content = System.IO.File.ReadAllBytes(file.LocalFileName);
                    var fileName = "ftm-" + file.Headers.ContentDisposition.FileName.Trim("\"".ToCharArray());

                    var fileUrl = _sharepointFileService.UploadFileToSharePoint(content, fileName);
                    //var fileUrl = _sharepointFileService.UploadFileToSharePoint(Server file., fileName);

                    if (fileUrl == null)
                    {
                        // Get any errors.
                        if (_sharepointFileService.Errors.Any())
                        {
                            return RetrieveErrorResult(_sharepointFileService.Errors);
                        }
                    }

                    // If there has not been a filegroup object created then create one.
                    // We need this for the fileGroupId that must go with every file.
                    fileGroup = fileGroup.fileGroupId == 0 ? _fileGroupService.Add(new fileGroup()) : fileGroup;
                    
                    // Create a new database file object to record where this file will be in sharepoint.
                    var databaseFile = new file()
                    {
                        url = fileUrl,
                        type = file.Headers.ContentType.MediaType,
                        name = fileName,
                        fileGroupId = fileGroup.fileGroupId
                    };

                    var newFile = _fileService.Add(databaseFile);

                    // This is set to null to elimante a self referencing loop problem.
                    newFile.fileGroup = null;

                    // Add the file objects to this list that is going to get returned to 
                    // the calling method.
                    databaseFiles.Add(newFile);
                }

                return Ok(databaseFiles);
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);

                //return InternalServerError(ex);

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

        //public HttpResponseMessage Get(string address, int? fileId = null)
        //{
        //    HttpResponseMessage result = null;

        //    // If there is a file id then we assume that the file is stored in the 
        //    // DEP Document management system and we try to retrieve it from there.
        //    if (fileId.HasValue)
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.OK);

        //        var depFile = _sharepointFileService.GetFile(fileId.Value);

        //        var byteArray = depFile.fileStream;

        //        result.Content = new ByteArrayContent(byteArray);
        //        result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //        result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        //        result.Content.Headers.ContentDisposition.FileName = depFile.fileName;
        //        result.Content.Headers.Add("x-filename", depFile.fileName);
        //    }
        //    else if (address != null)
        //    {
        //        // Serve the file to the client.
        //        result = Request.CreateResponse(HttpStatusCode.OK);
        //        result.Content = new StreamContent(new System.IO.FileStream(address, System.IO.FileMode.Open, System.IO.FileAccess.Read));
        //        result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //        result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        //        result.Content.Headers.ContentDisposition.FileName = address.Split("\\".ToCharArray()).LastOrDefault() ?? "Test.pdf";
        //        result.Content.Headers.Add("x-filename", address.Split("\\".ToCharArray()).LastOrDefault() ?? "Test.pdf");
        //    }
        //    else
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.Gone);
        //    }

        //    return result;
        //}

        //[HttpDelete]
        //public IHttpActionResult Delete(int id)
        //{
        //    _sharepointFileService.Remove(id);

        //    return Ok();
        //}

    }
}
