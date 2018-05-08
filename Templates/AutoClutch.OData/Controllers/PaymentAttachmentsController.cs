using AutoClutch.Core.Interfaces;
using $safeprojectname$.Core.Interfaces;
using $safeprojectname$.Core.Models;
using $safeprojectname$.DependencyResolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace $safeprojectname$.Controllers
{
    public class PaymentAttachmentsController : BaseApiController<paymentAttachment>
    {
        private IPaymentAttachmentService _paymentAttachmentService;

        public PaymentAttachmentsController(IPaymentAttachmentService paymentAttachmentService, ILogService<paymentAttachment> logService)
            : base(paymentAttachmentService, logService)
        {
            _paymentAttachmentService = paymentAttachmentService;
        }

    }
}
