using System.Collections.Generic;
using System.Threading.Tasks;
using $safeprojectname$.Objects;
using AutoClutch.Auto.Repo.Objects;

namespace $safeprojectname$.Interfaces
{
    public interface IDocumentService
    {
        IEnumerable<Error> Errors { get; set; }
        string ParentFolderName { get; set; }
        DEPFile Add(byte[] file, string fileName, long parentFolderId);
        DEPFile Add(DEPFile file);
        long AddFolder(string folderName);
        DEPFile GetFile(string fileName);
        DEPFile GetFile(int fileId);
        Task<DEPFile> GetFileAsync(int fileId);
        void InitialSetup();
        bool IsExists(string filename);
        void Remove(string filename);
        void Remove(int fileId);
        DEPFile Update(DEPFile file);
        Task<DEPFile> UpdateAsync(DEPFile file);
    }
}