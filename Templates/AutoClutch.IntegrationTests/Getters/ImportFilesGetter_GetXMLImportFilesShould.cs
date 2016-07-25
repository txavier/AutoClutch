using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Infrastructure.Getters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractTrackingManagement.DependencyResolution;
using ContractTrackingManagement.Core.Interfaces;

namespace ContractTrackingManagement.Infrastructure.Getters.Tests
{
    [TestClass()]
    public class ImportFilesGetter_GetXMLImportFilesShould
    {
        [TestMethod()]
        public void GetXMLImportFiles()
        {
            // Arrange.
            var container = IoC.Initialize();

            var importFilesGetter = container.GetInstance<IImportFilesGetter>();

            var directory = @"access-contractdb-xml";

            // Act.
            var fileNames = importFilesGetter.GetXMLImportFileNames(directory);

            // Assert.
            Assert.IsTrue(fileNames.Any());
        }
    }
}