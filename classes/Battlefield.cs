using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    /// <summary>
    /// field 
    /// 0 - empty
    /// 1 - ship tile
    /// 2 - missed shot
    /// 3 - hurt ship
    /// 4 - destroyed ship
    /// </summary>

    public class Battlefield
    {
        public List<Ship> ships;
        public List<Coords> shots;

        public string Name;

        public short[,] field = new short[10,10];

        public event EventHandler Ready;
        public event EventHandler Defeted;


        public Battlefield(string name)
        {
            shots = new List<Coords>();
            ships = new List<Ship>();
            Name = name;
            //addShip(new Ship(2, 4, 3, Direction.Horisontal));
            //addShip(new Ship(1, 1, 1, Direction.Horisontal));
            //addShip(new Ship(2, 7, 3, Direction.Vertical));
        }
        private bool addShipToField(Ship s)
        {
            foreach (Coords c in s.squares)
            {
                if (c.x > 9 || c.y > 9)
                    return false;
            }
            foreach (Coords c in s.squares)
            {
                field[c.x, c.y] = 1;
            }
            return true;
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

        public bool fieldReady()
        {
            if (this.ships.Count == 7)
                return true;
            return false;
        }

        public bool addShip(Ship s)
        {
            if (CheckCollision(s) && checkMaxShips(s.size))
            {
                
                if(!addShipToField(s))
                {
                    return false;
                }
                ships.Add(s);
                if (fieldReady())
                {
                    EventHandler tmp = Ready;
                    if(tmp != null)
                        Ready.Invoke(this, EventArgs.Empty);
                }
                return true;
            }
            else {
                return false;
            }
        }
        private bool checkMaxShips(int size)
        {
            int count = 0;
            foreach (Ship s in this.ships)
                if (s.size == size)
                    count++;
            if (size < 3)
            {
                if (count > 1)
                    return false;
                return true;
            }
            else
            {
                if (count > 0)
                    return false;
                return true;
            }
        }
        public HitResponse Hit(Coords c) 
        {
            HitResponse res = HitResponse.Miss;
            if (field[c.x, c.y] < 2)
            {
                
                foreach (Ship s in ships)
                {
                    res = s.CheckHit(c);
                    if (res == HitResponse.Hit)
                    {
                        
                        field[c.x, c.y] = 3;
                        break;
                    }
                    else if (res == HitResponse.Destroy)
                    {
                        foreach(Coords co in s.squares)
                            field[co.x, co.y] = 4;
                        break;
                    }

                }
                if(res == HitResponse.Miss)
                {
                    field[c.x, c.y] = 2;
                }
                }
            else
            {
                res = HitResponse.Error;
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
