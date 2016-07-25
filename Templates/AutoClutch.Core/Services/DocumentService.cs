using AutoClutch.Auto.Repo.Objects;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    public class DocumentService : IDocumentService
    {
        private IDocumentManagementServiceFacade _documentManagementServiceFacade;

        public string ParentFolderName { get; set; }

        public IEnumerable<Error> Errors { get; set; }

        public DocumentService(IDocumentManagementServiceFacade documentManagementServiceFacade)
        {
            _documentManagementServiceFacade = documentManagementServiceFacade;

            Errors = new List<Error>();
        }

        public void InitialSetup()
        {
            // Set the folder name.
            _documentManagementServiceFacade.CreateFolder(ParentFolderName);
        }

        public DEPFile GetFile(string fileName)
        {
            var document = _documentManagementServiceFacade.SearchFileByFilename(fileName).FirstOrDefault();

            return document;
        }

        public async Task<DEPFile> GetFileAsync(int fileId)
        {
            var result = await _documentManagementServiceFacade.GetDEPFileAsync(fileId);

            return result;
        }

        public DEPFile GetFile(int fileId)
        {
            var result = _documentManagementServiceFacade.GetDEPFile(fileId);

            return result;
        }

        /// <summary>
        /// Only use this method to add the initial contract tracking management folder.
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public Int64 AddFolder(string folderName)
        {
            long result = _documentManagementServiceFacade.CreateFolder(folderName);

            return result;
        }

        //public Int64 AddChildFolder(string childFolderName, Int64 parentFolderId)
        //{
        //    long result = _documentManagementServiceFacade.CreateChildFolder(childFolderName, parentFolderId);

        //    return result;
        //}

        //public IEnumerable<DepEnterpriseFolder> GetFolderFolders(Int64 folderId)
        //{
        //    DepClassLibrary.DocumentManagementWebService.DocumentObject[] documentObjectArray = documentManagementService.GetFolderContents(folderId);

        // var folderList = (from p in documentObjectArray where p.objectType ==
        // DepClassLibrary.DocumentManagementWebService.DocumentObjectType.Folder select new
        // DepEnterpriseFolder { folderId = p.objectId, folderName = p.objectName, parentFolderId =
        // folderId, //parentFolderName = documentManagementService.GetFoldernameParent(folderId) }).ToList();

        //    return folderList;
        //}

        //public IEnumerable<DEPFile> GetFolderFiles(Int64 folderId)
        //{
        //    DocumentObject[] documentObjectArray = _documentManagementServiceFacade.GetFolderContents(folderId);

        //    var fileList = (from p in documentObjectArray
        //                    where p.objectType == DocumentObjectType.File
        //                    select new DEPFile
        //                    {
        //                        fileId = p.objectId,
        //                        fileName = p.objectName,
        //                        fileStream = _documentManagementServiceFacade.GetFile(p.objectId),
        //                        childFolderId = folderId,
        //                    }).ToList();

        //    return fileList;
        //}

        public bool IsExists(string filename)
        {
            var result = _documentManagementServiceFacade.DoesFileExist(ParentFolderName, filename);

            return result;
        }

        public DEPFile Update(DEPFile file)
        {
            if (file.GetErrors().Any())
            {
                _documentManagementServiceFacade.UpdateFile(file.fileId, file.fileStream);
            }

            return file;
        }

        public async Task<DEPFile> UpdateAsync(DEPFile file)
        {
            if (file.GetErrors().Any())
            {
                await Task.Run(() => _documentManagementServiceFacade.UpdateFileAsync(file.fileId, file.fileStream));
            }

            return file;
        }

        public DEPFile Add(byte[] file, string fileName, long parentFolderId)
        {
            if (!RequirementCheck.Check(true, false, null, file))
            {
                ((List<Error>)Errors).Add(new Error { Description = "The file is missing a file id from the DEP Document Management Service", Property = "file" });

                return null;
            }

            RequirementCheck.Check(true, true, "The file is missing a file name.", fileName);

            var depFile = new Objects.DEPFile { fileName = fileName, fileStream = file, childFolderId = parentFolderId };

            depFile = this.Add(depFile);

            RequirementCheck.Check(true, true, "The file is missing a file id from the DEP Document Management Service.", depFile.fileId);

            return depFile;
        }

        public DEPFile Add(DEPFile file)
        {
            // The child folder id is needed.
            System.Diagnostics.Contracts.Contract.Assert(file.childFolderId != 0);

            if (file.GetErrors().Count == 0)
            {
                Int64 fileId = _documentManagementServiceFacade.PutFile(file.fileName, file.childFolderId, file.fileStream);

                file.fileId = fileId;
            }

            return file;
        }

        public void Remove(string filename)
        {
            var fileId = _documentManagementServiceFacade.DoesFileExistFileId(ParentFolderName, filename);

            if (!RequirementCheck.Check(true, false, null, fileId))
            {
                ((List<Error>)Errors).Add(new Error { Description = "Unable to find " + filename + " in the " + ParentFolderName + " folder.", Property = "filename" });

                return;
            }

            _documentManagementServiceFacade.DeleteFile(fileId);
        }

        public void Remove(int fileId)
        {
            _documentManagementServiceFacade.DeleteFile(fileId);
        }
    }
}