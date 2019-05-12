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
    class HumidityTempratureSensor : Sensor, IHumidity, IPressure
    {
        [DataMember]
        public double HumidityValue { get; set; }
        [DataMember]
        public double PressureValue { get; set; }

        public override string ToString()
        {
            return "The pressure value is: " + this.PressureValue + " and the humidity is: " + HumidityValue;
        }

        public override void TakeMeasurements()
        {
            base.TakeMeasurements();
            Random random = new Random();

            Dictionary<string, Double> dictionary = new Dictionary<string, double>();

            HumidityValue = random.Next(0, 200);
            PressureValue = random.Next(0, 200);
            
            dictionary.Add("humidity", HumidityValue);
            dictionary.Add("pressure", PressureValue);

            Measurement measurement = new Measurement(dictionary);

            this.OnMeasurementTaken(measurement);
        }
    }
}
