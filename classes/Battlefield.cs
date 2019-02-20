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


        public bool addShip(Ship s)
        {
            ships.Add(s);
            return true;
        }




    }



}
