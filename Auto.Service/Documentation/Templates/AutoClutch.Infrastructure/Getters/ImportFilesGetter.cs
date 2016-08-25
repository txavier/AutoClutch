using NerdLunch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace $safeprojectname$.Getters
{
    public class ImportFilesGetter : IImportFilesGetter
    {
        public ImportFilesGetter() { }

        public XElement GetXML(string fileName, string directoryString)
        {
            var xmlFileNames = GetXMLImportFileNames(directoryString);

            // Import Change Order
            var xmlFile = xmlFileNames.FirstOrDefault(i => i.Contains(fileName));

            var result = XElement.Load(xmlFile);

            return result;
        }

        public string[] GetXMLImportFileNames(string directory)
        {
            var fileNames = Directory.GetFiles(directory, "*.xml");

            return fileNames;
        }

        public string[] GetXSDImportFileNames(string directory)
        {
            var fileNames = Directory.GetFiles(directory, "*.xsd");

            return fileNames;
        }

        public XDocument GetXDocument(string xmlFilePath)
        {
            XDocument xdoc = XDocument.Load(xmlFilePath);

            return xdoc;
        }

        public XmlSchemaSet GetXmlSchema(string xsdFilePath)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();

            schemas.Add(null, xsdFilePath);

            return schemas;
        }

    }
}
