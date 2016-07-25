using System;
using System.Linq;
using System.Collections.Generic;
using AutoClutch.Auto.Repo.Objects;

namespace $safeprojectname$.Objects
{
    public class DEPFile
    {
        public byte[] fileStream { get; set; }

        public Int64 fileId { get; set; }

        public Guid fileGuid { get; set; }

        public string parentFolderName { get; set; }

        public string fileName { get; set; }

        public Int64 parentFolderId { get; set; }

        public string childFolderName { get; set; }

        public Int64 childFolderId { get; set; }

        public DateTime dateCreated { get; set; }

        public string mimeType { get; set; }

        public string ErrorString { get; set; }

        private List<Error> _ErrorList;

        public List<Error> ErrorList
        {
            get
            {
                if (_ErrorList == null)
                {
                    _ErrorList = GetErrors();
                }

                return _ErrorList;
            }
            set
            {
                _ErrorList = value;
            }
        }

        public List<Error> GetErrors()
        {
            List<Error> Errors = new List<Error>();


            if (string.IsNullOrWhiteSpace(this.fileName))
            {
                Errors.Add(new Error() { Description = "The filename must be supplied.", Property = "fileName" });
            }

            if (this.fileStream == null)
            {
                Errors.Add(new Error() { Description = "The fileStream must be supplied.", Property = "fileStream" });
            }

            //if (string.IsNullOrWhiteSpace(this.childFolderName))
            //{
            //    Errors.Add(new Error() { Description = "The child folder name for this file must be supplied.", Property = "childFolderName" });
            //}


            return Errors;
        }

        public bool IsValid()
        {
            if (this.GetErrors().Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetErrorsString()
        {
            if (!IsValid())
            {
                string result = GetErrors().Select(i => i.Description).Aggregate((current, next) => current + " " + next);

                return result;
            }

            return null;
        }
    }
}