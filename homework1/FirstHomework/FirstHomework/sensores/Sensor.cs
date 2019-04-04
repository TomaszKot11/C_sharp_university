using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading;

[DataContract]
public class Sensor
{

    // backing field 
    private String _name;

    // delegate definition
    public delegate void MeasurementTakenEventHandler(object source, EventArgs args);
    public event MeasurementTakenEventHandler MeasurementTaken;
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

    protected virtual void OnMeasurementTaken(EventArgs args)
    {
        if (MeasurementTaken != null)
            MeasurementTaken(this, args);
    }

}
