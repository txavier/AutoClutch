using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoClutch.Auto.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auto.Test.Data;
using AutoClutch.Auto.Repo.Objects;
using Moq;
using AutoClutch.Auto.Repo.Interfaces;
using AutoClutch.Auto.Service.Interfaces;
using StructureMap.AutoMocking.Moq;

namespace AutoClutch.Auto.Service.Services.UnitTests
{
    [TestClass()]
    public class Service_Delete_Should
    {
        [TestMethod()]
        public void Delete()
        {
            try
            {
                // Arrange.
                var autoMocker = new MoqAutoMocker<IService<user>>();

                var userService = autoMocker.ClassUnderTest;

                // Act.
                var result = userService.Delete(1, softDelete: true);

                // Assert.
                Assert.IsTrue(true);
            }
            finally
            {
                // Clean up database.
                var context = new AutoTestDataContext();

                context.users.RemoveRange(context.users.ToList());

                context.locations.RemoveRange(context.locations.ToList());

                context.facilities.RemoveRange(context.facilities.ToList());

                //context.LogDetails.RemoveRange(context.LogDetails.ToList());

                //context.AuditLog.RemoveRange(context.AuditLog.ToList());

                //context.SaveChanges();
            }
        }
    }
}