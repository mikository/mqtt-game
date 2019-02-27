using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace classes
{
    public class mqtt
    {
        //topic /ufct/blah

        MqttClient client;

        public event EventHandler gotHit;
        public event EventHandler gameStart;

        public void connect()
        {
            client = new MqttClient("134.41.136.157");
            client.Connect(Guid.NewGuid().ToString(), "dev", "dev1234");
           
            client.MqttMsgPublishReceived += getCommand;
            //WeaponFireComm c = new WeaponFireComm();
            //c.command = "sadnlasd";
            //c.x = 'A';
            //c.y = 1;
            //string message = JsonConvert.SerializeObject(c);
            //client.Publish("/ufct/blah", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        public void Subscribe(string topic)
        {
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        }

        public void SendCommand(Command c)
        {
            string message = JsonConvert.SerializeObject(c);
            client.Publish("/ufct/blah", Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }

        delegate void addShipCallBack(Coords c);

        public void close()
        {
            client.Disconnect();
        }

        private void getCommand(object sender, MqttMsgPublishEventArgs e)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
            var c = JsonConvert.DeserializeObject<Command>(Encoding.UTF8.GetString(e.Message), settings);
            if(c is WeaponFireComm)
            {
                WeaponFireComm co = (WeaponFireComm)c;
                gotHit.Invoke(new Coords(co.x, co.y), EventArgs.Empty);
            }
        }
    }


    public class Command
    {
        public string command { get; set; }
        internal Command(string c)
        {
            command = c;
        }
    }


    public class PingStartComm : Command
    {
        public string fleetName { get; set; }
        public PingStartComm(): base("PingStartComms"){}
    }
    public class PingResponseComm : Command
    {
        public string fleetName { get; set; }
        public string responseTo { get; set; }
        public PingResponseComm() : base("PingPingResponseComms") { }

    }
    public class GameStartComm : Command
    {
        public string regionName { get; set; }
        public GameStartComm() : base("GameStartComms") { }
    }
    public class GameResponseComm : Command
    {
        public string regionName { get; set; }
        public string fleetName { get; set; }
        public GameResponseComm() : base("GameResponseComms") { }
    }
    public class WeaponFireComm: Command
    {
        public char x { get; set; }
        public int y { get; set; }
        public WeaponFireComm() : base("WeaponFireComms") { }
    }
    public enum FireResponse
    {
        hit, miss
    }
    public enum FireResult
    {
        sunk, fleetdestroyed
    }
    public class WeaponResponseComm: Command
    {
        public char x { get; set; }
        public int y { get; set; }
        public FireResponse response { get; set; }
        public FireResult? result { get; set; }
        public WeaponResponseComm() : base("WeaponResponseComms") { }
    }
    public class WinnerDeclaredComm : Command
    {
        public string fleetName { get; set; }
        public WinnerDeclaredComm() : base("WinnerDeclaredComms") { }
    }
    public class WinnerDeclaredResponseComm:Command
    {
        public string fleetName { get; set; }
        public bool verified { get; set; }
        public WinnerDeclaredResponseComm() : base("WinnerDeclaredResponseComms") { }
    }
    public class WinnerConfirmedComm:Command
    {
        string fleetName { get; set; }
        public WinnerConfirmedComm() : base("WinnerConfirmedComms") { }
    }





}
