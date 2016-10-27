using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using $safeprojectname$.Models;
using $safeprojectname$.Core.Models;
using AutoClutch.Auto.Core.Interfaces;

namespace $safeprojectname$.Controllers
{
    [Authorize]
    public class makeController : ODataApiController<make>
    {
        //private ApplicationUserManager _userManager;

        public makeController(IService<make> makeService)
            : base(makeService)
        { }

        //public makeController(ApplicationUserManager userManager)
        //{
        //    UserManager = userManager;
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //// GET: api/make
        //public IEnumerable<string> Get()
        //{
        //    //var userIdentity = new ApplicationUser().GenerateUserIdentityAsync(UserManager).Result;

        //    var user = UserManager.Users.FirstOrDefault()?.UserName;

        //    var userId = User.Identity.GetUserId();

        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/make/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/make
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/make/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/make/5
        //public void Delete(int id)
        //{
        //}
    }
}
