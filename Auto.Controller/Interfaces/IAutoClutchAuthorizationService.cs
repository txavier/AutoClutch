using System.Collections.Generic;
using System.Security.Principal;

namespace Auto.Controller.Interfaces
{
    public interface IAutoClutchAuthorizationService
    {
        bool IsAuthorized(System.Security.Principal.IPrincipal User, string method, string absoluteUri);
        string GetLoggedInUserName(System.Security.Principal.IPrincipal User);
        IEnumerable<string> GetFilteredFields(IPrincipal user, string method, string absoluteUri, IEnumerable<string> fields);
    }
}