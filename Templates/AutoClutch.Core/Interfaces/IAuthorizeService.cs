using System.Collections.Generic;

namespace $safeprojectname$.Interfaces
{
    public interface IAuthorizeService
    {
        string IsAuthorized(string userName, bool? loginRequired, string permissionCheckType, string requiredPermissions, string uri, List<KeyValuePair<string, string>> parameters);
    }
}