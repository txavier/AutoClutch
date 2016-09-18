using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Auto.Test.Data;

namespace AutoClutch.Repo.Objects.Tests
{
    [TestClass()]
    public class Repository_ExistsShould
    {
        [TestMethod()]
        public void FindExistingItem()
        {
            try
            {
                var context = new AutoTestDataContext();

                // Arrange.
                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", location = location };

                var facilityRepository = new Repository<facility>(context);

                facilityRepository.Add(facility, "xingl");

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityRepository.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityRepository.Add(facility3, "theox");


                // Act.
                var found = facilityRepository.Exists(facility3);

                // Assert.
                Assert.IsTrue(found);
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
        public void NotFindItemThatDoesntExist()
        {
            try
            {
                var context = new AutoTestDataContext();

                // Arrange.
                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", location = location };

                var facilityRepository = new Repository<facility>(context);

                facilityRepository.Add(facility, "xingl");

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityRepository.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityRepository.Add(facility3, "theox");


                // Act.
                var found = facilityRepository.Exists(new facility() { facilityId = -1 });

                // Assert.
                Assert.IsTrue(!found);
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