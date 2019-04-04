using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Sensor sensor = new Sensor();
            //Console.WriteLine(sensor.Name);
            sensor.Name = "valid_name";
            Console.WriteLine(sensor.Name);
             try
            {
                sensor.Name = "This is not valid name which is not valid";
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // produce some sensores
            sensores.PressureSensor ps = new sensores.PressureSensor();
            ps.PressureValue = 3.4;
            sensores.HumidityTempratureSensor hps = new sensores.HumidityTempratureSensor();
            hps.HumidityValue = 45.6;
            hps.PressureValue = 1.2;
            sensores.TemperatureSensor ts = new sensores.TemperatureSensor();
            ts.MeasurementUnit = "Celcius";
            ts.MeasurementValue = 26.6;

            stations.WeatherStation ws = new stations.WeatherStation();
            ws.SerializationPeriod = 5000;
            ws.AddSensor(ps);
            ws.AddSensor(hps);
            ws.AddSensor(ts);
            ws.ReadMeasurement();

            ws.startTimer();

            // mock data to test filtering
            sensores.TemperatureSensor ts1 = new sensores.TemperatureSensor();
            ts1.MeasurementValue = 31;
            ts1.MeasurementUnit = "Celcius";
            sensores.TemperatureSensor ts2 = new sensores.TemperatureSensor();
            ts2.MeasurementValue = 35;
            ts2.MeasurementUnit = "Celcius";
            sensores.TemperatureSensor ts3 = new sensores.TemperatureSensor();
            ts3.MeasurementValue = 29;
            ts3.MeasurementUnit = "Celcius";

            ws.AddSensor(ts1);
            ws.AddSensor(ts2);
            ws.AddSensor(ts3);

            Func<Sensor, bool> samplePredicate = x => ((ITemperature)x).MeasurementValue > 30;
            Func<Sensor, bool> whichEleents = x => x is ITemperature;
            List<Sensor>  mySensores = ws.GetSpecificSensorTypesFullfillingGivenPredicate(whichEleents, samplePredicate);
            foreach(Sensor sensor1 in mySensores)
            {
                Console.WriteLine(sensor1);
            }

            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }
    }
}
