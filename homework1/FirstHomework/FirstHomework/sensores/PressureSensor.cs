using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using FirstHomework.Common;

namespace ConsoleApplication1.sensores
{
    [DataContract]
    class PressureSensor : Sensor, IPressure
    {
        [DataMember]
        public double PressureValue { get; set; }

        public override string ToString()
        {
            return "The pressure is: " + this.PressureValue;
        }

        public override void TakeMeasurements()
        {
            base.TakeMeasurements();

            Random random = new Random();
            
            Dictionary<string, Double> dictionary = new Dictionary<string, double>();

            PressureValue = random.Next(0, 202);
        
            dictionary.Add("pressure", PressureValue);

            Measurement measurement = new Measurement(dictionary);

           OnMeasurementTaken(measurement);
        }
    }
}
