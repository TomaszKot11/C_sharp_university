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
    class PressureSensor : Sensor, IPressure
    {
        [DataMember]
        public double PressureValue { get; set; }

        public override string ToString()
        {
            return "The pressure is: " + this.PressureValue;
        }
    }
}
