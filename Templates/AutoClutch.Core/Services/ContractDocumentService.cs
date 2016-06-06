using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClutch.Auto.Repo.Objects;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Objects;

namespace $safeprojectname$.Services
{
    public class ContractDocumentService : DocumentService, IContractDocumentService
    {
        private IDocumentManagementServiceFacade _documentManagementServiceFacade;

        private IEnvironmentConfigSettingsGetter _environmentConfigSettingsGetter;

        private long _parentFolderId;

        private string _parentFolderName;

        IEnumerable<Error> IContractDocumentService.Errors { get; set; }

        public ContractDocumentService(IDocumentManagementServiceFacade documentManagementServiceFacade, IEnvironmentConfigSettingsGetter environmentConfigSettingsGetter)
            : base(documentManagementServiceFacade)
        {
            _documentManagementServiceFacade = documentManagementServiceFacade;

            _environmentConfigSettingsGetter = environmentConfigSettingsGetter;

            _parentFolderName = _environmentConfigSettingsGetter.GetDocumentManagementSystemFolderName();

            if (!string.IsNullOrWhiteSpace(_parentFolderName))
            {
                _parentFolderId = documentManagementServiceFacade.GetTopLevelFolderId(_parentFolderName);
            }
        }

        public DEPFile Add(byte[] file, string fileName)
        {
            var result = base.Add(file, fileName, _parentFolderId);

            return result;
        }

        public long? Add(byte[] file, string filename, long? newFileId)
        {
            // If we have a new file to upload.
            if (file != null)
            {
                var depFile = Add(file, filename);

                RequirementCheck.Check(true, true, "The file is missing a file id from the DEP Document Management Service.", depFile.fileId);

                newFileId = depFile.fileId;
            }
            // Else if we have the file id of the file in the Document Management Service Web Service
            // and we have the file name then update the file in this webservice with this name.
            else if (!string.IsNullOrWhiteSpace(filename) && newFileId.HasValue)
            {
                // 'Update' the file with the correct file name.
                // The way we update here is to get the file contencts and save it as a new 
                // record with the Document Management Service with its updated filename
                // then delete the old record.
                var oldFile = GetFile((int)newFileId.Value);

                var depFile = Add(oldFile.fileStream, filename);

                Remove((int)oldFile.fileId);

                newFileId = depFile.fileId;
            }

            return newFileId;
        }
    }
}
