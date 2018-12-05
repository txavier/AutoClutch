﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoClutch.Test.Data;
using System;

namespace AutoClutch.Repo.Objects.Tests
{
    [TestClass()]
    public class Repository_UpdateShould
    {
        [TestMethod()]
        public void UpdateChildModels()
        {
            try
            {
                var context = new AutoTestDataContext();

                var context2 = new AutoTestDataContext();

                // Arrange.
                // Add a user.
                var userRepository = new EFRepository<user>(context);

                var user = new user() { name = "user1" };

                userRepository.Add(user);

                // Save user id.
                var userId = user.userId;

                // Add a location with that userId.
                var locationRepository = new EFRepository<location>(context);

                var location = new location() { name = "location1", contactUserId = user.userId };

                locationRepository.Add(location);

                // Add a facility with that locationId.
                var facilityRepository = new EFRepository<facility>(context);

                var facility = new facility() { name = "facility1", locationId = location.locationId };

                facilityRepository.Add(facility);

                // Get the facility back.
                var facilityRepository2 = new EFRepository<facility>(context2);

                facility = facilityRepository2.Get(filter: i => i.facilityId == facility.facilityId).SingleOrDefault();

                // Change the name of the user and put the object into the facility model.
                var user2 = new user();

                user2.userId = user.userId;

                user2.name = "user2";

                facility.location.user = user2;

                // Act.
                facilityRepository2.Update(facility);

                // Assert.
                facility = context2.facilities.SingleOrDefault(i => i.facilityId == facility.facilityId);

                Assert.AreEqual(facility.location.contactUserId, userId);

                Assert.AreEqual(facility.location.user.name, user2.name);
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
        public void NotAddDuplicates()
        {
            try
            {
                var context = new AutoTestDataContext();

                var context2 = new AutoTestDataContext();

                // Arrange.
                // Add a user.
                var userRepository = new EFRepository<user>(context);

                var user = new user() { name = "user1" };

                userRepository.Add(user);

                // Save user id.
                var userId = user.userId;

                // Add a location with that userId.
                var locationRepository = new EFRepository<location>(context);

                var location = new location() { name = "location1", contactUserId = user.userId };

                locationRepository.Add(location);

                var location2 = new location() { name = "location2", contactUserId = user.userId };

                locationRepository.Add(location2);

                // Add a facility with that locationId.
                var facilityRepository = new EFRepository<facility>(context);

                var facility = new facility() { name = "facility1", locationId = location.locationId };

                facilityRepository.Add(facility);

                // Get the facility back.
                var facilityRepository2 = new EFRepository<facility>(context2);

                facility = facilityRepository2.Get(filter: i => i.facilityId == facility.facilityId).SingleOrDefault();

                // Change the name of the user and put the object into the facility model.
                facility.locationId = location.locationId;

                // If facility.location is set to location2 then an exception will be thrown.  This is the correct behavior.
                facility.location = location;

                bool exceptionCaught = false;

                // Act.
                try
                {
                    facilityRepository2.Update(facility);
                }
                catch(Exception ex)
                {
                    // Assert.
                    Assert.IsTrue(true, "There was an exception caught when trying to add a duplicate to the database.  This is correct, duplicates are not being caught.");

                    exceptionCaught = true;
                }

                if(!exceptionCaught)
                {
                    Assert.IsFalse(false, "There was no exception caught when trying to add a duplicate to the database.  This is incorrect, duplicates are not being caught.");
                }


                //// Assert.
                //facility = context2.facilities.SingleOrDefault(i => i.facilityId == facility.facilityId);

                //Assert.AreEqual(facility.location.contactUserId, userId);
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
        public void Validate()
        {
            // Arrange.
            var context = new AutoTestDataContext();

            var userRepository = new EFRepository<user>(context);

            var user = new user() { name = "user1", userId = -1 };

            // Act.
            userRepository.Add(user);

            // Assert.
            Assert.IsTrue(userRepository.Errors.Any());
        }
    }
}
