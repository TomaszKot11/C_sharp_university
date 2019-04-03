using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

[DataContract]
public class Sensor
{

    // backing field 
    private String _name;
    public event ConsoleApplication1.Measurement measurement;
    public delegate void MeasurementDelegate(ConsoleApplication1.Measurement measurement);
   
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
    private static int counter;
    
   static Sensor()
   { 
        counter = 0;
   }

   

    public Sensor()
	{
        counter++;
        Name = "Sensor" + counter;
    }
}
