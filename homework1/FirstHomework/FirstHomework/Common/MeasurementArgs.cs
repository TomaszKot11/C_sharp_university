using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstHomework.Common
{
    public class MeasurementArgs : EventArgs
    {
        public ConsoleApplication1.Measurement Measurement { get; private set; }

        public MeasurementArgs(ConsoleApplication1.Measurement measurement)
        {
            this.Measurement = measurement;
        }


        public void PrintMeasurements()
        {
            Console.WriteLine(Measurement);
        }
    }
}
