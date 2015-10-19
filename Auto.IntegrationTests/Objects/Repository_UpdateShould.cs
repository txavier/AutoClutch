using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoClutch.Auto.Repo.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Auto.Test.Data;

namespace AutoClutch.Auto.Repo.Objects.Tests
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
                var userRepository = new Repository<user>(context);

                var user = new user() { name = "user1" };

                userRepository.Add(user);

                // Save user id.
                var userId = user.userId;

                // Add a location with that userId.
                var locationRepository = new Repository<location>(context);

                var location = new location() { name = "location1", contactUserId = user.userId };

                locationRepository.Add(location);

                // Add a facility with that locationId.
                var facilityRepository = new Repository<facility>(context);

                var facility = new facility() { name = "facility1", locationId = location.locationId };

                facilityRepository.Add(facility);

                // Get the facility back.
                var facilityRepository2 = new Repository<facility>(context2);

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
                var context = new AutoTestDataContext();

                context.users.RemoveRange(context.users.ToList());

                context.locations.RemoveRange(context.locations.ToList());

                context.facilities.RemoveRange(context.facilities.ToList());

                context.SaveChanges();
            }
        }
    }
}
