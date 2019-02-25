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
            client.Subscribe(new string[] { "/ufct/blah" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            client.MqttMsgPublishReceived += getCommand;
            WeaponFireComm c = new WeaponFireComm();
            c.command = "sadnlasd";
            c.x = 1;
            c.y = 1;
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
            //var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects}
            var c = JsonConvert.DeserializeObject<WeaponFireComm>(Encoding.UTF8.GetString(e.Message));
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
    }
    public class PingStartComm : Command
    {
        public string fleetName { get; set; }
    }
    public class PingResponseComm : Command
    {
        public string fleetName { get; set; }
        public string responseTo { get; set; }

    }
    public class GameStartComm : Command
    {
        public string regionName { get; set; }
    }
    public class GameResponseComm : Command
    {
        public string regionName { get; set; }
        public string fleetName { get; set; }
    }
    public class WeaponFireComm: Command
    {
        public int x { get; set; }
        public int y { get; set; }
    }



}
