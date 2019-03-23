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
    }
}
