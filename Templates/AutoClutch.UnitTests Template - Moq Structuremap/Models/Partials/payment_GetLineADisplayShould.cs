using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractTrackingManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractTrackingManagement.Core.Models.Tests
{
    [TestClass()]
    public class payment_GetLineADisplayShould
    {
        [TestMethod()]
        public void GetLineADisplay()
        {
            // Arrange.
            var deductionTypeLineHToA = new deductionType { name = "Line H to Line A" };

            var deductions = new List<deduction>()
            {
                new deduction
                {
                    paymentNumber = 3,
                    amount = (decimal)1195.00,
                    deductionType = deductionTypeLineHToA
                }
            };

            var payments = new List<payment>()
            {
            };

            var contract = new contract()
            {
                payments = payments
            };

            var payment1 = new payment()
            {
                paymentId = 284,
                paymentNumber = 1,
                paymentAmount = (decimal?)39274.04,
                contract = contract
            };
            var payment2 = new payment()
            {
                paymentId = 285,
                paymentNumber = 2,
                paymentAmount = (decimal?)71126.20,
                contract = contract
            };
            var payment3 = new payment()
            {
                paymentId = 286,
                paymentNumber = 3,
                paymentAmount = (decimal?)71517.77,
                deductions = deductions,
                contract = contract
            };
            //var payment4 = new payment()
            //{
            //    paymentId = 287,
            //    paymentNumber = 4,
            //    paymentAmount = (decimal?)10000,
            //    contract = contract
            //};

            payments.Add(payment1);
            payments.Add(payment2);
            payments.Add(payment3);
            //payments.Add(payment4);

            // Act.
            var result = contract.GetPaidToDateDisplay(contract.payments);

            // Assert.
            Assert.AreEqual(180723.01, result);
        }
    }
}