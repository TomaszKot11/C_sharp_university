using ConsoleApplication1.sensores;
using ConsoleApplication1.stations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static WeatherStation weatherStation = null;
        static Boolean ServerShouldRun = false;

        static void Main(string[] args)
        {
            weatherStation = new WeatherStation();

            // create few sensores
           // List<Sensor> sensorList = new List<Sensor>();

            HumidityTempratureSensor hs = new HumidityTempratureSensor();
            TemperatureSensor ts = new TemperatureSensor();
            TemperatureSensor ts1 = new TemperatureSensor();
            PressureSensor ps = new PressureSensor();

            ServerShouldRun = true;
            (new Thread(StartServerProc)).Start();

            // Take measurements
            ps.TakeMeasurements();
            hs.TakeMeasurements();
            ts1.TakeMeasurements();
            ts.TakeMeasurements();

            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
            ServerShouldRun = false;
        }

        static void StartServerProc()
        {
            weatherStation.StartServer();
            while(ServerShouldRun)
            {
                Thread.Sleep(500);
            }

            weatherStation.StopServer();
        }
    }
}
