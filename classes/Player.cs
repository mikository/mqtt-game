using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public class Player
    {
        public string fleetName;
        public Battlefield Battlefield;
        public bool lost;
        public string region;
        public bool ready;

        public event EventHandler Lost;

        public Player(string Fleet, string region)
        {
            fleetName = Fleet;
            this.region = region;
            lost = false;
            Battlefield = new Battlefield(Fleet);
        }


    }
}
