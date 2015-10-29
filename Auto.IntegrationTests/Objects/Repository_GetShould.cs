using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoClutch.Auto.Repo.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Test.Data;

namespace AutoClutch.Auto.Repo.Objects.Tests
{
    [TestClass()]
    public class Repository_GetShould
    {
        [TestMethod()]
        public void Get()
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
                var facility = new facility() { name = "facility1", location = location };

                var facilityRepository = new Repository<facility>(context);

                facilityRepository.Add(facility, "xingl");

                var newfacility = new facility();

                facility.name = "facility2";

                facilityRepository.Add(facility, "theox");


                // Act.
                var retrievedFacility = facilityRepository.Get(searchParameters: new List<string> { "name:facility2" });

                // Assert.
                Assert.IsTrue(retrievedFacility != null);
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