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

        private string gametopic;

        public event EventHandler PingRecieved;
        public event EventHandler PingResponseRecieved;
        public event EventHandler GameStartRecieved;
        public event EventHandler GameResponseRecieved;
        public event EventHandler ShotRecieved;
        public event EventHandler ShotResponseRecieved;

        //public event EventHandler gotHit;
        //public event EventHandler gameStart;
        public mqtt(string GameTopic)
        {
            gametopic = GameTopic;
        }
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
            Command command;
            Console.WriteLine(c.command);
            switch (c.command)
            {
                case "PingStartComms":
                    command = JsonConvert.DeserializeObject<PingStartComm>(Encoding.UTF8.GetString(e.Message), settings);
                    PingRecieved.Invoke(command, EventArgs.Empty);
                    break;
                case "PingPingResponseComms":
                    command = JsonConvert.DeserializeObject<PingResponseComm>(Encoding.UTF8.GetString(e.Message), settings);
                    PingResponseRecieved.Invoke(command, EventArgs.Empty);
                    break;
                case "GameStartComms":
                    command = JsonConvert.DeserializeObject<GameStartComm>(Encoding.UTF8.GetString(e.Message), settings);
                    GameStartRecieved.Invoke(command, EventArgs.Empty);
                    break;
                case "GameResponseComms":
                    command = JsonConvert.DeserializeObject<GameResponseComm>(Encoding.UTF8.GetString(e.Message), settings);
                    GameResponseRecieved.Invoke(command, EventArgs.Empty);
                    break;
                case "WeaponFireComms":
                    command = JsonConvert.DeserializeObject<WeaponFireComm>(Encoding.UTF8.GetString(e.Message), settings);
                    ShotRecieved.Invoke(command, EventArgs.Empty);
                    break;
                case "WeaponResponseComms":
                    command = JsonConvert.DeserializeObject<WeaponResponseComm>(Encoding.UTF8.GetString(e.Message), settings);
                    ShotResponseRecieved.Invoke(command, EventArgs.Empty);
                    break;
                default:
                    command = new Command("sdjasdasdjaksdj");
                    break;
            }
            
              //= JsonConvert.DeserializeObject <> (Encoding.UTF8.GetString(e.Message), settings);
        }

        private Type getCommandType(string command)
        {
            switch (command)
            {
                case "PingStartComms":
                    return typeof(PingStartComm);
                case "PingPingResponseComms":
                    return typeof(PingResponseComm);
                default:
                    return typeof(int);
            }
        }

        public void Ping(string region, Player p)
        {
            PingStartComm ping = new PingStartComm(p.fleetName);
            //ping.fleetName = p.fleetName;
            string msg = JsonConvert.SerializeObject(ping);
            client.Publish(gametopic + "/" + region, Encoding.ASCII.GetBytes(msg));
        }

        public void PingResponse(string region, Player respondTo, Player p)
        {
            PingResponseComm c = new PingResponseComm(p.fleetName, respondTo.fleetName);
            string msg = JsonConvert.SerializeObject(c);
            client.Publish(gametopic + "/" + region, Encoding.ASCII.GetBytes(msg));
        }

        public void StartGame(string region)
        {
            GameStartComm c = new GameStartComm(region);
            string msg = JsonConvert.SerializeObject(c);
            client.Publish(gametopic + "/" + region, Encoding.ASCII.GetBytes(msg));
        }

        public void RespondToGame(string region, string fleetName)
        {
            GameResponseComm c = new GameResponseComm(region, fleetName);
            string msg = JsonConvert.SerializeObject(c);
            client.Publish(gametopic + "/" + region, Encoding.ASCII.GetBytes(msg));
        }

        public void Shoot(char x, int y, string player, string region)
        {
            WeaponFireComm c = new WeaponFireComm(x, y);
            string msg = JsonConvert.SerializeObject(c);
            client.Publish(gametopic + "/" + region + "/" + player, Encoding.ASCII.GetBytes(msg));
        }

        public void RespondToShot(FireResponse resp, char x, int y, string player, string region, FireResult? result = null)
        {
            WeaponResponseComm c = new WeaponResponseComm(x, y, resp, result);
            string msg = JsonConvert.SerializeObject(c);
            client.Publish(gametopic + "/" + region + "/" + player, Encoding.ASCII.GetBytes(msg));
        }
    }


    public class Command
    {
        public string command { get; set; }
        [JsonConstructor]
        internal Command(string c)
        {
            command = c;
        }
    }


    public class PingStartComm : Command
    {
        public string fleetName { get; set; }
        public PingStartComm(string fleetName): base("PingStartComms")
        {
            this.fleetName = fleetName;
        }
    }
    public class PingResponseComm : Command
    {
        public string fleetName { get; set; }
        public string responseTo { get; set; }
        public PingResponseComm(string fleetName, string responseTo) : base("PingPingResponseComms")
        {
            this.fleetName = fleetName;
            this.responseTo = responseTo;
        }

    }
    public class GameStartComm : Command
    {
        public string regionName { get; set; }
        public GameStartComm(string regionName) : base("GameStartComms")
        {
            this.regionName = regionName;
        }
    }
    public class GameResponseComm : Command
    {
        public string regionName { get; set; }
        public string fleetName { get; set; }
        public GameResponseComm(string regionName, string fleetName) : base("GameResponseComms")
        {
            this.regionName = regionName;
            this.fleetName = fleetName;
        }
    }
    public class WeaponFireComm: Command
    {
        public char x { get; set; }
        public int y { get; set; }
        public WeaponFireComm(char x, int y) : base("WeaponFireComms")
        {
            this.x = x;
            this.y = y;
        }
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
        public WeaponResponseComm(char X, int Y, FireResponse res, FireResult? result) : base("WeaponResponseComms")
        {
            x = X;
            y = Y;
            response = res;
            this.result = result;
        }
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
