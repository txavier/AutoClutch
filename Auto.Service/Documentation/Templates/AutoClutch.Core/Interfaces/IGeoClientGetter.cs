using System.Threading.Tasks;
using $safeprojectname$.Objects.GeoClient;

namespace $safeprojectname$.Interfaces
{
    public interface IGeoClientGetter
    {
        Task<RootObject> GetGeoClient(string address);
    }
}