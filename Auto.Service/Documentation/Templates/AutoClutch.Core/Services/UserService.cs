using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Services
{
    public class UserService
    {
        public static string GetFullNameCalculated(string firstName, string lastName)
        {
            var result = ((firstName ?? string.Empty) + " " + (lastName ?? string.Empty)).Trim();

            return result;
        }
    }
}
