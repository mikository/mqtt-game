using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public class Game
    {
        List<Player> Players = new List<Player>();
        string gametopic;
        string region;
        mqtt m;


        public Game()
        {
            m = new mqtt();
            gametopic = "battleship";
            region = "NBCC";

        }

        public Game(Player p, string topic)
        {
            if (p.ready)
            {

            }
        }

        private void Subscribe(string topic)
        {

        }
    }
}
