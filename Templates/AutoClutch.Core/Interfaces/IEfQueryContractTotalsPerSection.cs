using $safeprojectname$.ViewModels;
using System.Collections.Generic;

namespace $safeprojectname$.Interfaces
{
    public interface IEfQueryContractTotalsPerSection
    {
        IEnumerable<GraphViewModel> QueryContractTotalsPerSection();
    }
}