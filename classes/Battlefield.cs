using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    class Battlefield
    {
        List<Ship> ships = new List<Ship>();
        List<Coords> shots = new List<Coords>();

        public Battlefield()
        {
            ships.Add(new Ship(2, 1, 1, Direction.Horisontal));
            ships.Add(new Ship(3, 2, , Direction.Vertical));
        }

        public bool addShip(Ship s)
        {
            ships.Add(s);
            return true;
        }

        public HitResponse Hit(Coords c) 
        {
            HitResponse res = HitResponse.Miss;
            foreach(Ship s in ships)
            {
                res = s.CheckHit(c);
                if (res != HitResponse.Miss)
                    return res;
            }
            return res;
        }

        public List<Coords> GetCoords()
        {

        }



    }



}
