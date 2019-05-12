using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Timers;
using ConsoleApplication1.sensores;
using FirstHomework.Common;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

// serializing list to json in c# 
// https://stackoverflow.com/questions/9110724/serializing-a-list-to-json
// deserialization
//https://stackoverflow.com/questions/2316397/sending-and-receiving-custom-objects-using-tcpclient-class-in-c-sharp/2316483
namespace ConsoleApplication1.stations
{
    public class MeasurementEventArgs
    {

        //public SampleEventArgs(string s) { Text = s; }
        //public String Text { get; } // readonly
    }


    public class ClientHandler
    {
        private TcpClient ClientSocket;
        private string ClientNumber;
        private WeatherStation WeatherStation;
        private Mutex mutex = Mutex.OpenExisting(WeatherStation.MUTEX_GUID);

        public void StartClient(TcpClient ClientSocket, string ClientNumber, WeatherStation WeatherStation)
        {
            this.ClientSocket = ClientSocket;
            this.ClientNumber = ClientNumber;
            this.WeatherStation = WeatherStation;

            Thread thread = new Thread(HandleMeasurement);
            thread.Start();
        }

        //TODO: mutex
        private void HandleMeasurement()
        {
            // handle the measurement
            NetworkStream networkStream = ClientSocket.GetStream();
            IFormatter formatter = new BinaryFormatter();
            Measurement measurement = (Measurement)formatter.Deserialize(networkStream);
            WeatherStation.AddMeasurement(measurement);

            Console.WriteLine("Client: " + ClientNumber + " obtained: " + measurement.ToString());

            networkStream.Close();
            ClientSocket.Close();
            WeatherStation.UnsubscribeClient(ClientSocket);
        }
    }

    [DataContract]
    [KnownType(typeof(PressureSensor))]
    [KnownType(typeof(TemperatureSensor))]
    [KnownType(typeof(HumidityTempratureSensor))]
    public class WeatherStation
    {

        #region NETWORK_COMMUNICATION
        public static string MUTEX_GUID = "e1ffff8f-c91d-4188-9e82-c92ca5b1d057";
        public const Int32 PORT = 8888;
        public const string IP_ADDRESS = "127.0.0.1";
        private const string JSON_FILE_NAME = "measurements.json";
        private TcpListener server = null;
        private Mutex mutex = null;
        private Boolean ServerRunning = false;
        #endregion

        [DataMember]
        private List<Sensor> parts = new List<Sensor>();

        private List<Measurement> measurements = new List<Measurement>();
        List<TcpClient> clients = new List<TcpClient>();


        [DataMember]
        public double _serializationPeriod;
        [DataMember]
        public double SerializationPeriod
        {
            get
            {
                return _serializationPeriod;
            }

            set
            {
                if (value < 0) throw new Exception("Value should be greater than 0");
                _serializationPeriod = value;
            }
        }

        private System.Timers.Timer aTimer;

        public void AddMeasurement(Measurement measurement)
        {
            measurements.Add(measurement);
        }

        public void UnsubscribeClient(TcpClient client)
        {
            clients.Remove(client);
        }

        public WeatherStation()
        {
            this.SerializationPeriod = 5000;
            //startTimer();
        }

        ~WeatherStation()
        {
            if(aTimer != null)
                aTimer.Stop();
        }

        // starts multithreading server 
        public void StartServer()
        {
            IPAddress localAddr = IPAddress.Parse(IP_ADDRESS);
            this.server = new TcpListener(localAddr, PORT);
            this.mutex = new Mutex(false, MUTEX_GUID);

            server.Start();
            Console.WriteLine("-------SERVER STARTED-------");
            this.ServerRunning = true;

            while (ServerRunning)
            {
                TcpClient client = server.AcceptTcpClient();
                clients.Add(client);

                // start the thread for the incomming connection
                ClientHandler clientHandler = new ClientHandler();
                clientHandler.StartClient(client, clients.FindIndex(c => c == client).ToString(), this);

            }
            try
            {
                foreach (TcpClient client in clients)
                    client.Close();
                    
                this.server.Stop();
            } catch(SocketException e)
            {
                Console.WriteLine("Error while closing the socket!");
                Console.WriteLine(e);
            }

            Console.WriteLine("-------SERVER STOPPED-------");
        }

        public void StopServer()
        {
            if(ServerRunning)
                this.ServerRunning = false;
        }

        public void AddSensor(Sensor sensor)
        {
            parts.Add(sensor);
        }

        public void ReadMeasurement()
        {
            foreach (Sensor sensor in parts)
            {
                Console.WriteLine("--------------");
                Console.WriteLine(sensor);
                Console.WriteLine("\n--------------");
            }
        }


        public void startTimer()
        {
            this.aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(this.OnTimeEvent);
            aTimer.Interval = this.SerializationPeriod;
            aTimer.Enabled = true;

            prepareDirectory();
            aTimer.Start();
        }


        private void prepareDirectory()
        {
            string settings_dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "homework_jsons");
            Directory.CreateDirectory(settings_dir);
        }

        private void OnTimeEvent(object source, ElapsedEventArgs e)
        {
            // SerialzieToJson();
            SerializeMeasurementsToJson();
        }

        //TODO: mutex when writing
        // TODO: synchronizacja + apeend:)
        private void SerializeMeasurementsToJson()
        {
            TextWriter writer = null;

            try
            {
                var json = JsonConvert.SerializeObject(this.measurements);
                string pathToJsons = string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "homework_jsons", JSON_FILE_NAME);
                writer = new StreamWriter(pathToJsons, false);
              //  mutex.WaitOne();
                writer.Write(json);
               // this.measurements.Clear();
               // mutex.ReleaseMutex();
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private void SerialzieToJson()
        {

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(WeatherStation));
            MemoryStream msObj = new MemoryStream();
            js.WriteObject(msObj, this);
            msObj.Position = 0;

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".json";
            var pathToJsons = string.Format("{0}\\{1}\\{2}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "homework_jsons", fileName);
            using (FileStream fs = new FileStream(pathToJsons, FileMode.OpenOrCreate))
            {
                msObj.CopyTo(fs);
                fs.Flush();
            }


            // sample read
            msObj.Position = 0;
            StreamReader sr = new StreamReader(msObj);
            string json = sr.ReadToEnd();
            Console.WriteLine("JSON: " + json + " was written to the file!");
            sr.Close();
            msObj.Close();
        }

        public List<Sensor> GetSpecificSensorTypesFullfillingGivenPredicate(Func<Sensor, bool> whichTypes, Func<Sensor, bool> predicate)
        {
            return parts.Where(whichTypes).Where(predicate).ToList();
        }
    }
}
