using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractTrackingManagement.Controllers.Tests
{
    [TestClass()]
    public class BaseApiController_GetDynamicQueryShould
    {
        [TestMethod()]
        public void GetDynamicQueryWithExistingQuery()
        {
            // Arrange.
            var currentQuery = "ContractNumber=\"Test-Contract2\"";

            var fullTextSearchFields = "contractNumber,contractDescription";

            var fullTextSearchParameters= "1236-BIO,201-hey";

            // Act.
            var result = Controllers.BaseApiController<Core.Models.contract>.GetDynamicQuery(fullTextSearchParameters, fullTextSearchFields, currentQuery);

            // Assert.
            // contractNumber.Contains("20151427556") and section.sectionId=1
            Assert.AreEqual("ContractNumber=\"Test-Contract2\" AND ((contractNumber.Contains(\"1236-BIO\") OR contractDescription.Contains(\"1236-BIO\")) OR (contractNumber.Contains(\"201-hey\") OR contractDescription.Contains(\"201-hey\")))", result);
        }

        [TestMethod()]
        public void GetDynamicQueryWithoutExistingQuery()
        {
            // Arrange.
            var fullTextSearchFields = "contractNumber,contractDescription";

            var fullTextSearchParameters = "1236-BIO,201-hey";

            // Act.
            var result = Controllers.BaseApiController<Core.Models.contract>.GetDynamicQuery(fullTextSearchParameters, fullTextSearchFields);

            // Assert.
            // contractNumber.Contains("20151427556") and section.sectionId=1
            Assert.AreEqual("(contractNumber.Contains(\"1236-BIO\") OR contractDescription.Contains(\"1236-BIO\")) OR (contractNumber.Contains(\"201-hey\") OR contractDescription.Contains(\"201-hey\"))", result);
        }

        [TestMethod()]
        public void GetDynamicQueryWithoutExistingFields()
        {
            // Arrange.
            var fullTextSearchParameters = "1236-BIO,201-hey";

            // Act.
            var result = Controllers.BaseApiController<Core.Models.contract>.GetDynamicQuery(fullTextSearchParameters, type: typeof(Core.Models.contract));

            // Assert.
            // contractNumber.Contains("20151427556") and section.sectionId=1
            Assert.AreEqual("(contractNumber.Contains(\"1236-BIO\") OR contractDescription.Contains(\"1236-BIO\") OR replacementContractNumber.Contains(\"1236-BIO\") OR replacementContractNumber2.Contains(\"1236-BIO\") OR specification.Contains(\"1236-BIO\") OR originalContractNumber.Contains(\"1236-BIO\") OR renewalContractNumber.Contains(\"1236-BIO\") OR registrationNumber.Contains(\"1236-BIO\") OR projectLaborAgreement.Contains(\"1236-BIO\") OR cancelationReason.Contains(\"1236-BIO\")) OR (contractNumber.Contains(\"201-hey\") OR contractDescription.Contains(\"201-hey\") OR replacementContractNumber.Contains(\"201-hey\") OR replacementContractNumber2.Contains(\"201-hey\") OR specification.Contains(\"201-hey\") OR originalContractNumber.Contains(\"201-hey\") OR renewalContractNumber.Contains(\"201-hey\") OR registrationNumber.Contains(\"201-hey\") OR projectLaborAgreement.Contains(\"201-hey\") OR cancelationReason.Contains(\"201-hey\"))", result);
        }

        [TestMethod()]
        public void GetDynamicQueryWithExistingQueryWithoutExistingFields()
        {
            // Arrange.
            var currentQuery = "ContractNumber=\"Test-Contract2\"";

            var fullTextSearchParameters = "1236-BIO,201-hey";

            // Act.
            var result = Controllers.BaseApiController<Core.Models.contract>.GetDynamicQuery(fullTextSearchParameters, currentQuery: currentQuery, type: typeof(Core.Models.contract));

            // Assert.
            // contractNumber.Contains("20151427556") and section.sectionId=1
            Assert.AreEqual("ContractNumber=\"Test-Contract2\" AND ((contractNumber.Contains(\"1236-BIO\") OR contractDescription.Contains(\"1236-BIO\") OR replacementContractNumber.Contains(\"1236-BIO\") OR replacementContractNumber2.Contains(\"1236-BIO\") OR specification.Contains(\"1236-BIO\") OR originalContractNumber.Contains(\"1236-BIO\") OR renewalContractNumber.Contains(\"1236-BIO\") OR registrationNumber.Contains(\"1236-BIO\") OR projectLaborAgreement.Contains(\"1236-BIO\") OR cancelationReason.Contains(\"1236-BIO\")) OR (contractNumber.Contains(\"201-hey\") OR contractDescription.Contains(\"201-hey\") OR replacementContractNumber.Contains(\"201-hey\") OR replacementContractNumber2.Contains(\"201-hey\") OR specification.Contains(\"201-hey\") OR originalContractNumber.Contains(\"201-hey\") OR renewalContractNumber.Contains(\"201-hey\") OR registrationNumber.Contains(\"201-hey\") OR projectLaborAgreement.Contains(\"201-hey\") OR cancelationReason.Contains(\"201-hey\")))", result);
        }

    }
}