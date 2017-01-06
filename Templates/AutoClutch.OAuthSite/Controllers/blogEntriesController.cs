using WebX.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutch.Core.Interfaces;
using AutoClutch.Controller;

namespace $safeprojectname$.Controllers
{
    public class blogEntriesController : ODataApiController<blogEntry>
    {
        public blogEntriesController(IService<blogEntry> blogEntryService, ILogService<blogEntry> blogEntryLogService)
            : base(blogEntryService, blogEntryLogService)
        { }
    }
}