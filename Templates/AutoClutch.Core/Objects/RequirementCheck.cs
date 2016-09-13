using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Objects
{
    public static class RequirementCheck
    {
        public static bool Check(bool returnFalseOnDefaultValueType = false, bool throwArgumentNullException = false, string exceptionMessage = null, params object[] list)
        {
            foreach (var requiredItem in list)
            {
                var typeString = requiredItem.GetType().Name;

                // If this is null or if this value is an int and equals 0 then this requirement has not passed.
                if (requiredItem == null || (returnFalseOnDefaultValueType && (requiredItem.Equals(GetDefaultValue(requiredItem.GetType())))))
                {
                    if (throwArgumentNullException)
                    {
                        var paramName = typeString;

                        exceptionMessage = exceptionMessage ?? "Unable to continue, the " + paramName + " is required.";

                        throw new ArgumentOutOfRangeException(paramName, exceptionMessage);
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <see cref="http://stackoverflow.com/questions/2686678/in-net-at-runtime-how-to-get-the-default-value-of-a-type-from-a-type-object"/>
        public static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }
    }
}
