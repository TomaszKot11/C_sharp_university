using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstHomework.Common
{
    public class MeasurementArgs : EventArgs
    {
        private ConsoleApplication1.Measurement Measurement;

        public MeasurementArgs(ConsoleApplication1.Measurement measurement)
        {
            this.Measurement = measurement;
        }
    }
}
