using System.Collections.Generic;

namespace $safeprojectname$.Interfaces
{
    public interface IMetricService
    {
        IEnumerable<ViewModels.GraphViewModel> QueryContractTotalsPerSection();
    }
}