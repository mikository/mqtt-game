using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public enum PlayerStatus
    {
        Setting,
        Redy,
        InGame
    }

    public class Player
    {
        public string fleetName;
        public Battlefield Battlefield;
        public PlayerStatus Status { get; private set; }
        public bool lost;
        public bool ready;

        public event EventHandler Lost;

        public Player(string Fleet)
        {
            fleetName = Fleet;
            lost = false;
            Battlefield = new Battlefield(Fleet);
            Status = PlayerStatus.Setting;
        }
        public bool isFleetDestroyed()
        {
            foreach(Ship s in Battlefield.ships)
            {
                if (!s.destroyed)
                    return false;
            }
            return true;
        }

    }
}
