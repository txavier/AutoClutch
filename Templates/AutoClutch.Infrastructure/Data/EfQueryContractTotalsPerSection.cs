using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Data
{
    public class EfQueryContractTotalsPerSection : IEfQueryContractTotalsPerSection
    {
        private EfDataDbContext _dbContext;
        public EfQueryContractTotalsPerSection(EfDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<GraphViewModel> QueryContractTotalsPerSection()
        {
            var result = _dbContext.engineers
                .Select(i => new GraphViewModel
                {
                    label = i.name,
                    data = new Random().Next()
                });

            return result;
        }
    }
}
