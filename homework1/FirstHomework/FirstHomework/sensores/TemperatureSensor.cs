using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ConsoleApplication1.sensores
{
    [DataContract]
    class TemperatureSensor : Sensor, ITemperature
    {

        [DataMember]
        public double Measurement { get; set; }
        [DataMember]
        public string MeasurementUnit { get; set; }

        public override string ToString()
        {
            return "The temperature is: " + this.Measurement + " " + this.MeasurementUnit;
        }
    }
}
