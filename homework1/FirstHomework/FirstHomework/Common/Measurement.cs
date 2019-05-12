using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    [Serializable]
    public class Measurement
    {
        public Dictionary<string, double> measurements { get; private set; }

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

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(KeyValuePair<string, double> entry in measurements)
            {
                stringBuilder.Append(entry.Key + " " + entry.Value+" ");
            }

           
            return stringBuilder.ToString();
        }


    }
}
