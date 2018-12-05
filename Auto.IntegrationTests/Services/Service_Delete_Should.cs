using AutoClutch.Core;
using AutoClutch.Repo;
using AutoClutch.Test.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace AutoClutch.IntegrationTests.Services
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
                var context = new AutoTestDataContext();

                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", location = location };

                var facilityRepository = new EFRepository<facility>(context);

                var facilityService = new EFService<facility>(facilityRepository);

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
                var result = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContext())).Delete(facility3.facilityId, loggedInUserName: "IntegrationTest", softDelete: true);

                var shouldNotHaveIt = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContext())).Queryable()
                    .Where(i => i.facilityId == facility3.facilityId);

                var shouldHaveIt = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContext())).Queryable(includeSoftDeleted: true)
                    .Where(i => i.facilityId == facility3.facilityId);

                // Assert.
                Assert.IsFalse(shouldNotHaveIt.Any());

                Assert.IsTrue(shouldHaveIt.Any());
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

        [TestMethod()]
        public void DeleteAsync()
        {
            try
            {
                // Arrange.
                var context = new AutoTestDataContext();

                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", location = location };

                var facilityRepository = new EFRepository<facility>(context);

                var facilityService = new EFService<facility>(facilityRepository);

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
                var result = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContext())).DeleteAsync(facility3.facilityId, "theox", softDelete: true).Result;

                var shouldNotHaveIt = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContext())).Queryable()
                    .Where(i => i.facilityId == facility3.facilityId);

                var shouldHaveIt = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContext())).Queryable(includeSoftDeleted: true)
                    .Where(i => i.facilityId == facility3.facilityId);

                // Assert.
                Assert.IsFalse(shouldNotHaveIt.Any());

                Assert.IsTrue(shouldHaveIt.Any());
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
