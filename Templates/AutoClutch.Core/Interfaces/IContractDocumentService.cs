using AutoClutch.Auto.Repo.Objects;
using $safeprojectname$.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface IContractDocumentService
    {
        IEnumerable<Error> Errors { get; set; }
        string ParentFolderName { get; set; }
        DEPFile Add(byte[] file, string fileName);
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
        long? Add(byte[] file, string filename, long? newFileId);
    }
}
