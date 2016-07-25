using AutoClutch.Auto.Repo.Objects;
using $safeprojectname$.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace $safeprojectname$.Services
{
    public class XmlValidatorService : IXmlValidatorService
    {
        private IImportFilesGetter _importFilesGetter;

        public List<Error> Errors { get; }

        public XmlValidatorService(IImportFilesGetter importFilesGetter)
        {
            _importFilesGetter = importFilesGetter;

            Errors = new List<Error>();
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/bb387037.aspx
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xsdFilePath"></param>
        /// <returns></returns>
        public bool IsValidXml(string xmlFilePath, string xsdFilePath)
        {
            var xdoc = _importFilesGetter.GetXDocument(xmlFilePath);

            var schemas = _importFilesGetter.GetXmlSchema(xsdFilePath);

            Console.WriteLine("Attempting to validate");

            try
            {
                bool errors = false;

                List<string> messages = new List<string>();

                xdoc.Validate(schemas, (o, e) =>
                {
                    Console.WriteLine("{0}", e.Message);

                    errors = true;

                    messages.Add(e.Message);
                });

                Console.WriteLine(xmlFilePath + " {0}", (errors ? "did not validate." : "validated."));

                ((List<AutoClutch.Auto.Repo.Objects.Error>)Errors).Add(new AutoClutch.Auto.Repo.Objects.Error
                {
                    Description = string.Format(xmlFilePath + " {0}", errors ? "did not validate." : "validated."),
                });

                if (errors)
                {
                    return false;
                }
            }
            catch (XmlSchemaValidationException e)
            {
                return false;
            }

            return true;
        }
    }
}
