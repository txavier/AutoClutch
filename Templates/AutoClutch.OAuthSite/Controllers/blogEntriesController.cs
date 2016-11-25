using Auto.Controller.Objects;
using WebX.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoClutch.Core.Interfaces;

namespace $safeprojectname$.Controllers
{
    public class blogEntriesController : ODataApiController<blogEntry>
    {
        public blogEntriesController(IService<blogEntry> blogEntryService)
            : base(blogEntryService)
        { }

        //public IHttpActionResult Get()
        //{
        //    return Ok(User.Identity.Name);
        //}
    }
}