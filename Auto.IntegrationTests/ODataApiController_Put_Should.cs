using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoClutch.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClutch.Repo;
using AutoClutch.Test.Data;
using AutoClutch.Core;
using System.Web.Http.Results;

namespace AutoClutch.Controller.Tests
{
    [TestClass()]
    public class ODataApiController_Put_Should
    {
        [TestMethod()]
        public void GetUpdatedObjectBack()
        {
            try
            {
                var context = new AutoTestDataContextNonTrackerEnabled();

                // Arrange.
                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", location = location };

                var facilityRepository = new Repository<facility>(context);

                var facilityService = new Service<facility>(facilityRepository);

                facilityService.Add(facility, "xingl");

                var facilityODataController = new ODataApiController<facility>(facilityService);

                facility.name = "facility1x";

                // Act.
                var result = facilityODataController.Put(facility.facilityId, facility).Result;

                // Assert.
                Assert.IsTrue(result != null);

                var posRes = result as OkNegotiatedContentResult<object>;

                Assert.AreNotEqual("facility1x", posRes.Content);
            }
            finally
            {
                // Clean up database.
                var context = new AutoTestDataContextNonTrackerEnabled();

                context.users.RemoveRange(context.users.ToList());

                context.locations.RemoveRange(context.locations.ToList());

                context.facilities.RemoveRange(context.facilities.ToList());

                context.SaveChanges();

                var context2 = new AutoTestDataContext();

                context2.LogDetails.RemoveRange(context2.LogDetails.ToList());

                context2.AuditLog.RemoveRange(context2.AuditLog.ToList());

                context2.SaveChanges();
            }
        }
    }
}