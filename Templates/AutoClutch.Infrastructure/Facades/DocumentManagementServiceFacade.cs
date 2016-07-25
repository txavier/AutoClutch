using $safeprojectname$.DocumentManagementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Facades
{
    public class DocumentManagementServiceFacade : Service1, IDocumentManagementServiceFacade
    {
        public new IEnumerable<DEPFile> SearchFileByFilename(string filename)
        {
            var files = base.SearchFileByFilename(filename);

            var depFiles = files.Select(i => ToDEPFile(i));

            return depFiles;
        }

        public async Task<DEPFile> GetDEPFileAsync(long fileId)
        {
            var task = await Task.Run(() => GetDEPFile(fileId));

            return task;
        }

        public DEPFile GetDEPFile(long fileId)
        {
            // Set the time out to two minutes.
            base.Timeout = 300000;

            var fileDetails = base.GetFileDetails(fileId);

            var depFile = new DEPFile
            {
                fileStream = base.GetFile(fileId),
                fileName = fileDetails.objectDetailName,
                fileId = fileId,
                dateCreated = fileDetails.objectDateCreated,
            };

            return depFile;
        }

        private DEPFile ToDEPFile(FileObject fileObject)
        {
            return new DEPFile
            {
                fileStream = base.GetFile(fileObject.FileId),
                parentFolderName = this.GetFoldernameParent(fileObject.FileId),
                parentFolderId = this.GetFileParent(fileObject.FileId),
                fileName = fileObject.FileName,
                fileId = fileObject.FileId,
                dateCreated = fileObject.DateCreated
            };
        }

    }
}
