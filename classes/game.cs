using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public class Game
    {
        public List<Player> Players = new List<Player>();  //code for multi player for future implementation
        public Player me;

        public Player opponent = null;
        string gametopic;
        string region;

        bool myTurn = false;

        bool lookingForGame = false;

        mqtt m;


        public event EventHandler OpponentFound;
        public event EventHandler Lost;
        public event EventHandler Won;


        public Game()
        {
            m = new mqtt("battleship");
            gametopic = "battleship";
            region = "NBCC";
        }


        public Game(string gametopic, string region,  Player p, string fleet)
        {
            this.gametopic = gametopic;
            this.me = p;
            this.region = region;
            m = new mqtt(gametopic);
            m.connect();
            m.Subscribe(gametopic + "/" + region);
            m.Subscribe(gametopic + "/" + region + "/" + fleet);
            m.PingRecieved += M_PingRecieved;
            m.PingResponseRecieved += M_PingResponseRecieved;
            m.GameStartRecieved += M_GameStartRecieved;
            m.GameResponseRecieved += M_GameResponseRecieved;
            m.ShotRecieved += M_ShotRecieved;
            m.ShotResponseRecieved += M_ShotResponseRecieved;
            //opponent = new Player("Olena");
            if (me.ready)
            {
                PingStartComm c = new PingStartComm(fleet);
                sendRegionCommand(c);
            }
        }

        private void M_ShotResponseRecieved(object sender, EventArgs e)
        {
            if (myTurn)
            {
                WeaponResponseComm c = (WeaponResponseComm)sender;
                short field = 0;
                if (c.result == null)
                {
                    if (c.response == FireResponse.hit)
                    {
                        myTurn = true;
                        field = 3;
                    }
                    else
                    {
                        field = 2;
                        myTurn = false;
                    }
                }
                else if (c.result == FireResult.sunk)
                {
                    myTurn = true;
                    field = 4;
                }
                else     //implement o win
                {
                    Won.Invoke(me, EventArgs.Empty);
                }

                opponent.Battlefield.field[getIntFromLetter(c.x), c.y] = field;
            }
        }

        private void M_ShotRecieved(object sender, EventArgs e)
        {
            if (!myTurn)
            {
                WeaponFireComm c = (WeaponFireComm)sender;
                HitResponse res = me.Battlefield.Hit(new Coords(getIntFromLetter(c.x), c.y));

                switch (res)
                {
                    case HitResponse.Hit:
                        m.RespondToShot(FireResponse.hit, c.x, c.y, opponent.fleetName, this.region);
                        myTurn = false;
                        break;
                    case HitResponse.Miss:
                        m.RespondToShot(FireResponse.miss, c.x, c.y, opponent.fleetName, this.region);
                        myTurn = true;
                        break;
                    case HitResponse.Destroy:
                        myTurn = false;
                        m.RespondToShot(FireResponse.hit, c.x, c.y, opponent.fleetName, this.region, me.isFleetDestroyed() ? FireResult.fleetdestroyed : FireResult.sunk);
                        break;
                    default:
                        break;
                }
                if (me.isFleetDestroyed())
                    Lost.Invoke(me, EventArgs.Empty);
                    
            }

        }

        private void M_GameResponseRecieved(object sender, EventArgs e)
        {
            if (lookingForGame)
            {
                GameResponseComm c = (GameResponseComm)sender;
                if (c.fleetName == me.fleetName)
                    return;
                else
                {
                    
                    opponent = new Player(c.fleetName);
                    OpponentFound.Invoke(opponent, EventArgs.Empty);
                    lookingForGame = false;
                    
                }
            }
        }

        private void M_GameStartRecieved(object sender, EventArgs e)
        {
            if (lookingForGame)
            {
                m.RespondToGame(this.region, me.fleetName);
            }
        }

        private void M_PingResponseRecieved(object sender, EventArgs e)
        {
            if (lookingForGame)
            {
                PingResponseComm c = (PingResponseComm)sender;
                if(c.responseTo == me.fleetName)
                {
                    //opponent = new Player(c.fleetName);
                    StartGame(new Player(c.fleetName));
                    
                }
            }
        }

        private void M_PingRecieved(object sender, EventArgs e)
        {
            if (lookingForGame)
            {
                PingStartComm c = (PingStartComm)sender;
                
                if (c.fleetName != me.fleetName)
                {
                    //opponent = new Player(c.fleetName);
                    m.PingResponse(this.region, new Player(c.fleetName), this.me);
                }
            }
        }

        private void StartGame(Player opponent)    //method to start game with specific opponent temporary mvp 
        {
            m.StartGame(this.region);
            myTurn = true;
        }

        public void Start()
        {
            lookingForGame = true;
                Ping(me);
        }
        //to implement
        private void sendPersonalCommand(Command c, Player p) { }

        public void Soot(Coords c, Player plyer)
        {
                m.Shoot(getLetterFromInt(c.x), c.y, plyer.fleetName, this.region);
        }

        private char getLetterFromInt(int i)
        {
            return (char)(i +65);
        }
        private int getIntFromLetter(char c)
        {
            return (int)c - 65;
        }

        private void sendRegionCommand(Command c)
        {

        }

        private void Ping(Player p)
        {
            m.Ping(this.region, p);
        }
        
    }
}
