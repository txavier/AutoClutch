using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.Core.Services;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    public class PaymentsODataController : ODataApiController<payment>
    {
        private IPaymentService _paymentService;

        public PaymentsODataController(IPaymentService paymentService, ILogService<payment> logService)
            : base(paymentService, logService)
        {
            _paymentService = paymentService;
        }

        //copied
        public IHttpActionResult RunBusinessRules(payment payment)
        {
            var paymentFromDatabase = _paymentService.Queryable()
                .Include("contract.payments")
                .Include("deductions.deductionType")
                .Include("paymentType")
                .SingleOrDefault(i => i.paymentId == payment.paymentId);

            paymentFromDatabase.paymentType = payment.paymentType;

            paymentFromDatabase.projectRetainage = payment.projectRetainage;

            paymentFromDatabase.paymentAmount = payment.paymentAmount;

            var pay = new
            {
                lineADisplay = paymentFromDatabase.lineADisplay,
                lineBDisplay = paymentFromDatabase.lineBDisplay,
                lineCDisplay = paymentFromDatabase.lineCDisplay,
                lineEDisplay = paymentFromDatabase.lineEDisplay,
                lineFDisplay = paymentFromDatabase.lineFDisplay,
                lineGDisplay = paymentFromDatabase.lineGDisplay,
                lineHDisplay = paymentFromDatabase.lineHDisplay,
                lineIDisplay = paymentFromDatabase.lineIDisplay,
            };

            return Ok(pay);
        }

        //copied
        public async Task<IHttpActionResult> DeletePaymentFile(int paymentId)
        {
            var result = await _paymentService.DeletePaymentFileAsync(paymentId, User.Identity.Name.Split("\\".ToCharArray()).LastOrDefault());

            return Ok();
        }

        //copied
        public async Task<IHttpActionResult> GetInitialPayment(int contractId)
        {
            var result = await _paymentService.GetInitialPaymentAsync(contractId);

            return Ok(result);
        }
    }
}
