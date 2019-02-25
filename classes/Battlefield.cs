using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public class Battlefield
    {
        public List<Ship> ships;
        public List<Coords> shots;

        public short[,] field = new short[10,10];

        public Battlefield()
        {
            shots = new List<Coords>();
            ships = new List<Ship>();
            //addShip(new Ship(2, 4, 3, Direction.Horisontal));
            //addShip(new Ship(1, 1, 1, Direction.Horisontal));
            //addShip(new Ship(2, 7, 3, Direction.Vertical));
        }
        private void addShipToField(Ship s)
        {
            foreach(Coords c in s.squares)
            {
                field[c.x, c.y] = 1;
            }
        }
        private void initEmptyField()
        {
            for(int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    field[j, i] = 0;
                }
            }
        }

        public bool addShot(Coords c)
        {

            shots.Add(c);
            field[c.x, c.y] = 2;
            return true;
        }

        private bool CheckCollision(Ship newShip)
        {
            foreach(Ship s in this.ships)
            {
                if(s.Collides(newShip))
                    return false;
            }
            return true;
        }

        public bool addShip(Ship s)
        {
            if (CheckCollision(s))
            {
                ships.Add(s);
                addShipToField(s);
                return true;
            }
            else {
                return false;
            }
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
            List<Coords> res = new List<Coords>();
            foreach (Ship c in ships)
            {
                res.AddRange(c.squares);
            }
            return res;
        }



    }
}
