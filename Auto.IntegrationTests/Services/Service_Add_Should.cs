using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using AutoClutch.Test.Data;
using AutoClutch.Repo;
using AutoClutch.Core;
using AutoClutch.Core.Objects;
using System;
using System.Collections.Generic;
using AutoClutch.Core.Interfaces;

namespace AutoClutch.Service.Services.IntegrationTests
{
    public class CustomValidation<TEntity> : IValidation<TEntity>
        where TEntity : class, new()
    {
        public IEnumerable<Error> Errors
        {
            get; set;
        }

        public CustomValidation()
        {
            Errors = new List<Error>();
        }

        public bool IsValid(TEntity entity, string loggedInUserName = null, IService<TEntity> service = null)
        {
            ((List<Error>)Errors).Add(new Error { Description = "Validation event", Property = "Property" });

            Errors = ((List<Error>)Errors).Distinct().ToList();

            return false;
        }
    }

    [TestClass()]
    public class Service_Add_Should
    {
        [TestMethod()]
        public void ValidateGovernmentFacility()
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

                var validation = new CustomValidation<facility>();

                var facilityService = new Service<facility>(facilityRepository);

                facilityService.Validation = validation;

                // Act.
                facilityService.Add(facility, "xingl");

                // Assert.
                Assert.IsFalse(facilityService.IsValid(facility));

                Assert.IsTrue(facilityService.Errors.Any());

                Assert.IsTrue(facilityService.Errors.FirstOrDefault().Description == "Validation event");
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
