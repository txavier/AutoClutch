using System.Xml.Linq;
using System.Xml.Schema;

namespace $safeprojectname$.Interfaces
{
    public interface IImportFilesGetter
    {
        string[] GetXMLImportFileNames(string directory);
        string[] GetXSDImportFileNames(string directory);
        XElement GetXML(string fileName, string directoryString);
        XDocument GetXDocument(string xmlFilePath);
        XmlSchemaSet GetXmlSchema(string xsdFilePath);
    }
}