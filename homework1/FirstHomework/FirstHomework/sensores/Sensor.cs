using ConsoleApplication1;
using ConsoleApplication1.stations;
using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Threading;

[DataContract]
public class Sensor
{

    // backing field 
    private String _name;
    private static int counter;

    public String Name
    {
        get
        {
            return _name;
        }
        set
        {
           if (value.Length > 16) throw new Exception("Name " + value + " cannot be longer than 16 characters!");
           _name = value;
        }
    }
   
    
    static Sensor()
    { 
        counter = 0;
    }
   
    public Sensor()
	{
        counter++;
        Name = "Sensor" + counter;
    }

    public virtual void TakeMeasurements()
    {
        Console.WriteLine("Taking measurement...");

        Thread.Sleep(3000);

    }

    protected virtual void OnMeasurementTaken(Measurement measurement)
    {
        TcpClient tcpClient = new TcpClient();
        try
        {
            tcpClient.Connect(WeatherStation.IP_ADDRESS, WeatherStation.PORT);

            Console.WriteLine("Conencted to the server!");

            NetworkStream networkStream = tcpClient.GetStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(networkStream, measurement);

            networkStream.Close();
            tcpClient.Close();
        } catch(SocketException e)
        {
            Console.WriteLine("Error while connecting to the server");
            Console.WriteLine(e);
        }
    }
}
