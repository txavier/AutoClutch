using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Objects
{
    public class MetricsData
    {
        public IEnumerable<string> labels { get; set; }

        public List<string> series { get; set; }

        public List<List<double>> data { get; set; }

        public double highest;

        public double lowest;

        public MetricsData()
        {
            labels = new HashSet<string>();

            series = new List<string>();

            data = new List<List<double>>();

            highest = new double();

            lowest = new double();
        }
    }
}