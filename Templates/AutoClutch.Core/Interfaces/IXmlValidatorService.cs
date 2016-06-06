using AutoClutch.Auto.Repo.Objects;
using System.Collections.Generic;

namespace $safeprojectname$.Interfaces
{
    public interface IXmlValidatorService
    {
        bool IsValidXml(string xmlFilePath, string xsdFilePath);

        List<Error> Errors { get; }
    }
}