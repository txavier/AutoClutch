﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using AutoClutch.Test.Data;

namespace AutoClutch.Repo.Objects.Tests
{
    [TestClass()]
    public class Repository_NonTrackerEnabled_GetShould
    {
        [TestMethod()]
        public void GetTwoGovernmentFacilityRecords()
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
                var retrievedFacility = facilityRepository.Get(filterString: "facilityType=\"Government\"");

                // Assert.
                Assert.IsTrue(retrievedFacility != null);

                Assert.AreEqual(2, retrievedFacility.Where(i => i.facilityType == "Government").Count());
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
        public void GetOneGovernmentRecordAndSkipOneGovernemntRecord()
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

                var facilityRepository = new EFRepository<facility>(context);

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
                var retrievedFacility = facilityRepository.Get(filterString: "facilityType=\"Government\"", skip: 1);

                // Assert.
                Assert.IsTrue(retrievedFacility != null);

                Assert.AreEqual(1, retrievedFacility.Where(i => i.facilityType == "Government").Count());
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
        public void OrderAllFacilityRecordsInDescendingOrderByName()
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

                var facilityRepository = new EFRepository<facility>(context);

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
                var retrievedFacility = facilityRepository.Get(filterString: "facilityType=\"Government\"", orderByString: "name descending");

                // Assert.
                Assert.IsTrue(retrievedFacility != null);

                Assert.AreEqual("facility3", retrievedFacility.First().name);
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

                var facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                facilityRepository.Add(facility, "xingl");

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityRepository.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityRepository.Add(facility3, "theox");

                facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                // Act.
                var retrievedFacility = facilityRepository.Get(filterString: "facilityType=\"Commercial\"", lazyLoadingEnabled: false, proxyCreationEnabled: false, autoDetectChanges: false);

                var testtwo = retrievedFacility.FirstOrDefault();

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
        public void GetTwoGovernmentFacilityRecordsWithOutContextLevelProxy()
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

                var facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                facilityRepository.Add(facility, "xingl");

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityRepository.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityRepository.Add(facility3, "theox");

                facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                facilityRepository.ProxyCreationEnabled = false;

                facilityRepository.LazyLoadingEnabled = false;

                facilityRepository.AutoDetectChangesEnabled = false;

                // Act.
                var retrievedFacility = facilityRepository.Get(filterString: "facilityType=\"Commercial\"");

                var testtwo = retrievedFacility.FirstOrDefault();

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
        public void GetSkipZeroTakeTwoGovernmentFacilityRecordsWithOutProxy2()
        {
            try
            {
                var context = new AutoTestDataContextNonTrackerEnabled();

                // Arrange.
                // Add a user.
                var user = new user() { name = "user1" };

                // Add a location with that userId.
                var location = new location() { name = "location1", user = user };

                var locationRepository = new EFRepository<location>(new AutoTestDataContext());

                location = locationRepository.Add(location);

                // Add a facility with that locationId.
                var facility = new facility() { name = "facility1", facilityType = "Commercial", locationId = location.locationId };

                var facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                // Act.
                facility = facilityRepository.Add(facility, "xingl");

                // Assert.
                Assert.IsTrue(facility.facilityId != 0);

                Assert.IsTrue(facility.locationId != 0);
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
        public void GetSkipZeroTakeTwoGovernmentFacilityRecordsWithOutProxy()
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

                var facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                facilityRepository.Add(facility, "xingl");

                var newfacility = new facility();

                newfacility.name = "facility2";

                newfacility.facilityType = "Government";

                facilityRepository.Add(newfacility, "theox");

                var facility3 = new facility();

                facility3.name = "facility3";

                facility3.facilityType = "Government";

                facilityRepository.Add(facility3, "theox");

                facilityRepository = new EFRepository<facility>(new AutoTestDataContextNonTrackerEnabled());

                facilityRepository.ProxyCreationEnabled = false;

                facilityRepository.LazyLoadingEnabled = false;

                // Act.
                var retrievedFacility = facilityRepository.Get(filterString: "facilityType=\"Commercial\"", skip: 0, take: 2);

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
    }
}