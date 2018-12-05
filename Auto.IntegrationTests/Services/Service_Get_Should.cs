using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using AutoClutch.Test.Data;
using AutoClutch.Repo;
using AutoClutch.Core;

namespace AutoClutch.Service.Services.IntegrationTests
{
    [TestClass()]
    public class Service_Get_Should
    {
        [TestMethod()]
        public void GetTwoGovernmentFacilityRecordsWithOutProxy()
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

                var facilityRepository = new EFRepository<facility>(context);

                var facilityService = new EFService<facility>(facilityRepository);

                facilityService.Add(facility, "xingl");

                //facilityService.Dispose();

                //context = new AutoTestDataContextNonTrackerEnabled();

                //facilityRepository = new Repository<facility>(context);

                //facilityService = new Service<facility>(facilityRepository);

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityService.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityService.Add(facility3, "theox");

                facilityService.ProxyCreationEnabled = false;

                facilityService.LazyLoadingEnabled = false;

                var newFacilityService = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled()));

                newFacilityService.ProxyCreationEnabled = true;

                newFacilityService.LazyLoadingEnabled = true;

                var newFacilityService2 = new EFService<facility>(new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled()));

                newFacilityService2.ProxyCreationEnabled = false;

                newFacilityService2.LazyLoadingEnabled = false;

                // Act.
                var retrievedFacility1 = newFacilityService.Get(filter: i => i.facilityType.Contains("Commercial"));

                var retrievedFacility = newFacilityService2.Get(filter: i => i.facilityType.Contains("Commercial"));

                var retrievedFacility2 = facilityService.Get(filter: i => i.facilityType.Contains("Commercial"));

                // Assert.
                Assert.IsTrue(retrievedFacility != null);

                Assert.AreEqual(null, retrievedFacility.First().location);
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
        public void GetTwoGovernmentFacilityRecordsWithProxy()
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

                var facilityRepository = new EFRepository<facility>(context);

                var facilityService = new EFService<facility>(facilityRepository);

                facilityService.Add(facility, "xingl");

                facilityService.Dispose();

                context = new AutoTestDataContextNonTrackerEnabled();

                facilityRepository = new EFRepository<facility>(context);

                facilityService = new EFService<facility>(facilityRepository);

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityService.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityService.Add(facility3, "theox");
                
                facilityService.ProxyCreationEnabled = true;

                facilityService.LazyLoadingEnabled = true;

                // Act.
                var retrievedFacility = facilityService.Get(filterString: "facilityType=\"Commercial\"");

                // Assert.
                Assert.IsTrue(retrievedFacility != null);

                Assert.AreNotEqual(null, retrievedFacility.First().location);
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
