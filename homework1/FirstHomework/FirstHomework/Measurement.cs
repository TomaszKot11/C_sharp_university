using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Measurement
    {
        private Dictionary<string, double> measurements;

        public Measurement()
        {
            measurements = new Dictionary<string, double>();
        }
        
        public Measurement(Dictionary<string, double> initialMeasurements) : this()
        {
            AddMeasurements(initialMeasurements);
        }

        public void AddMeasurements(Dictionary<string, double> Measurements)
        {
            Measurements.ToList().ForEach(x => measurements.Add(x.Key, x.Value));
        }


    }
}
