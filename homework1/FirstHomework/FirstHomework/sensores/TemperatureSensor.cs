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
    class TemperatureSensor : Sensor, ITemperature
    {

        [DataMember]
        public double MeasurementValue { get; set; }
        [DataMember]
        public string MeasurementUnit { get; set; }

        public override string ToString()
        {
            return "The temperature is: " + this.MeasurementValue + " " + this.MeasurementUnit;
        }


        public override void TakeMeasurements()
        {
            base.TakeMeasurements();
            string[] units = new string[] { "Celcius", "Fahrenheit" };
            Random random = new Random();
            int randomIndex = random.Next(0, units.Length);
            Dictionary<string, Double> dictionary = new Dictionary<string, double>();

            MeasurementUnit = units[randomIndex];
            MeasurementValue = random.Next(0, 200);

            dictionary.Add(MeasurementUnit, MeasurementValue);

            Measurement measurement = new Measurement(dictionary);

            OnMeasurementTaken(measurement);
        }
    }
}
