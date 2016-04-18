using Auto.Test.Data;
using AutoClutch.Auto.Repo.Objects;
using AutoClutch.Auto.Service.Interfaces;
using AutoClutch.Auto.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.IntegrationTests.Services
{
    [TestClass]
    public class Service_Delete_Should
    {
        [TestMethod()]
        public void Delete()
        {
            try
            {
                // Arrange.
                var context = new AutoTestDataContextNonTrackerEnabled();

                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", location = location };

                var facilityRepository = new Repository<facility>(context);

                var facilityService = new Service<facility>(facilityRepository);

                facilityService.Add(facility, "xingl");

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityService.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityService.Add(facility3, "theox");

                // Act.
                var result = new Service<facility>(new Repository<facility>(new AutoTestDataContextNonTrackerEnabled())).Delete(facility3.facilityId, softDelete: true);

                var shouldNotHaveIt = new Service<facility>(new Repository<facility>(new AutoTestDataContextNonTrackerEnabled())).Queryable()
                    .Where(i => i.facilityId == facility3.facilityId);

                var shouldHaveIt = new Service<facility>(new Repository<facility>(new AutoTestDataContextNonTrackerEnabled())).Queryable(includeSoftDeleted: true)
                    .Where(i => i.facilityId == facility3.facilityId);

                // Assert.
                Assert.IsFalse(shouldNotHaveIt.Any());

                Assert.IsTrue(shouldHaveIt.Any());
            }
            finally
            {
                // Clean up database.
                var context = new AutoTestDataContext();

                context.users.RemoveRange(context.users.ToList());

                context.locations.RemoveRange(context.locations.ToList());

                context.facilities.RemoveRange(context.facilities.ToList());

                context.LogDetails.RemoveRange(context.LogDetails.ToList());

                context.AuditLog.RemoveRange(context.AuditLog.ToList());

                context.SaveChanges();
            }
        }
    }
}
