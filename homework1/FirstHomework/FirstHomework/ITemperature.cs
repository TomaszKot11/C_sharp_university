﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    interface ITemperature
    {
        double MeasurementValue { get; set; }
        string MeasurementUnit { get; set; }
        
    }
}
