using $safeprojectname$.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace $safeprojectname$.Interfaces
{
    public interface IDocumentManagementServiceFacade
    {
        string Url { get; set; }
        bool UseDefaultCredentials { get; set; }

        void CancelAsync(object userState);
        long CreateChildFolder(string childFolderName, long parentFolderId);
        void CreateChildFolderAsync(string childFolderName, long parentFolderId);
        void CreateChildFolderAsync(string childFolderName, long parentFolderId, object userState);
        long CreateFolder(string foldername);
        void CreateFolderAsync(string foldername);
        void CreateFolderAsync(string foldername, object userState);
        void DeleteFile(long fileId);
        void DeleteFileAsync(long fileId);
        void DeleteFileAsync(long fileId, object userState);
        bool DoesFileExist(string topFolderName, string filename);
        void DoesFileExistAsync(string topFolderName, string filename);
        void DoesFileExistAsync(string topFolderName, string filename, object userState);
        long DoesFileExistFileId(string topFolderName, string filename);
        void DoesFileExistFileIdAsync(string topFolderName, string filename);
        void DoesFileExistFileIdAsync(string topFolderName, string filename, object userState);
        //DocumentObject[] GetAllTopLevelFolders();
        void GetAllTopLevelFoldersAsync();
        void GetAllTopLevelFoldersAsync(object userState);
        long GetChildFolderId(string folderName, long parentFolderId);
        void GetChildFolderIdAsync(string folderName, long parentFolderId);
        void GetChildFolderIdAsync(string folderName, long parentFolderId, object userState);
        long GetChildFolderIdByParentName(string folderName, string parentFolderName);
        void GetChildFolderIdByParentNameAsync(string folderName, string parentFolderName);
        void GetChildFolderIdByParentNameAsync(string folderName, string parentFolderName, object userState);
        byte[] GetFile(long fileId);
        void GetFileAsync(long fileId);
        void GetFileAsync(long fileId, object userState);
        //DocumentObjectDetail GetFileDetails(long fileId);
        void GetFileDetailsAsync(long fileId);
        void GetFileDetailsAsync(long fileId, object userState);
        //DocumentObjectDetail[] GetFileDetailsList(long[] fileIds);
        void GetFileDetailsListAsync(long[] fileIds);
        void GetFileDetailsListAsync(long[] fileIds, object userState);
        string GetFilenameFromId(long fileId);
        void GetFilenameFromIdAsync(long fileId);
        void GetFilenameFromIdAsync(long fileId, object userState);
        long GetFileParent(long fileID);
        void GetFileParentAsync(long fileID);
        void GetFileParentAsync(long fileID, object userState);
        //DocumentObject[] GetFolderContents(long folderId);
        void GetFolderContentsAsync(long folderId);
        void GetFolderContentsAsync(long folderId, object userState);
        //DocumentObjectDetail[] GetFolderContentsDetail(long folderId);
        void GetFolderContentsDetailAsync(long folderId);
        void GetFolderContentsDetailAsync(long folderId, object userState);
        string GetFoldernameParent(long fileID);
        void GetFoldernameParentAsync(long fileID);
        void GetFoldernameParentAsync(long fileID, object userState);
        long GetFolderParent(long folderID);
        void GetFolderParentAsync(long folderID);
        void GetFolderParentAsync(long folderID, object userState);
        long GetTopLevelFolderId(string folderName);
        void GetTopLevelFolderIdAsync(string folderName);
        void GetTopLevelFolderIdAsync(string folderName, object userState);
        long PutFile(string fileName, long folderId, [XmlElement(DataType = "base64Binary")] byte[] fileStream);
        void PutFileAsync(string fileName, long folderId, byte[] fileStream);
        void PutFileAsync(string fileName, long folderId, byte[] fileStream, object userState);
        //FileObject[] SearchFileByFilename(string filename);
        IEnumerable<DEPFile> SearchFileByFilename(string filename);
        void SearchFileByFilenameAsync(string filename);
        void SearchFileByFilenameAsync(string filename, object userState);
        void UpdateFile(long fileId, [XmlElement(DataType = "base64Binary")] byte[] fileStream);
        void UpdateFileAsync(long fileId, byte[] fileStream);
        void UpdateFileAsync(long fileId, byte[] fileStream, object userState);
        Task<DEPFile> GetDEPFileAsync(long fileId);
        DEPFile GetDEPFile(long fileId);
    }
}