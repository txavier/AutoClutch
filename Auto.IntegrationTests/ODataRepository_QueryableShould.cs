using Auto.Test.Data.Models;
using AutoClutch.Repo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xml2CSharp;

namespace Auto.IntegrationTests
{
    [TestClass()]
    public class ODataRepository_QueryableShould
    {
        [TestMethod()]
        public void QueryCaseStatuses()
        {
            // Arrange.
            var caseStatusRepository = new ODataRepository<CaseStatus>();

            caseStatusRepository.SetUri("https://data.cityofnewyork.us/api/odata/v4/jz4z-kudi");

            // Act.
            var result = caseStatusRepository.Queryable();

            var newResult = result.Take(1).ToList();

            // Assert.
            Assert.IsTrue(newResult.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(newResult.FirstOrDefault()?.ticket_number));
        }

        [TestMethod()]
        public void QueryDEPIWCByIssuingAgency()
        {
            try
            {
                // Arrange.
                var caseStatusRepository = new AtomXmlRepository<Properties>("http://data.cityofnewyork.us/OData.svc/jz4z-kudi");

                // Act.
                var result = caseStatusRepository.Queryable().Where(i => i.Issuing_agency == "DEP - IWC");

                var newResult = result.Take(2);

                // Assert.
                Assert.IsTrue(newResult.Count() > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(newResult.FirstOrDefault()?.Issuing_agency));
                Assert.AreEqual(newResult.FirstOrDefault().Issuing_agency, "DEP - IWC");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod()]
        public void QueryDEPIWCByNovNumber()
        {
            try
            {
                // Arrange.
                var caseStatusRepository = new AtomXmlRepository<Properties>("http://data.cityofnewyork.us/OData.svc/jz4z-kudi");

                // Act.
                var result = caseStatusRepository.Queryable().Where(i => i.Ticket_number == String.Format("{0:0000000000}", 204792015));

                // Assert.
                Assert.IsTrue(result.Count() > 0);
                Assert.IsFalse(string.IsNullOrWhiteSpace(result.FirstOrDefault()?.Issuing_agency));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TestMethod()]
        public void TopOfV4Endpoint()
        {
            // Arrange.
            var caseStatusRepository = new ODataRepository<CaseStatus>();

            // https://data.cityofnewyork.us/OData.svc/jz4z-kudi
            caseStatusRepository.SetUri("https://data.cityofnewyork.us/api/odata/v4/jz4z-kudi");

            // Act.
            var result = caseStatusRepository.Queryable().Take(1);

            var newResult = result.ToList();

            // Assert.
            Assert.IsTrue(newResult.Count == 1);
        }

        [TestMethod()]
        public void BeQueryable()
        {
            // Arrange.
            var caseStatusRepository = new ODataRepository<CaseStatus>();

            // https://data.cityofnewyork.us/OData.svc/jz4z-kudi
            caseStatusRepository.SetUri("https://data.cityofnewyork.us/api/odata/v4/jz4z-kudi");

            // Act.
            var result = caseStatusRepository.Queryable();

            var newResult = result.Take(15).ToList();

            // Assert.
            Assert.IsTrue(newResult.Count > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(newResult.FirstOrDefault()?.ticket_number));
        }
    }
}
