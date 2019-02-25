using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFMLFront
{
    class Field
    {
        public int X;
        public int Y;
        public int Player;
        public Field(int x, int y, int p)
        {
            X = x;
            Y = y;
            Player = p;
        }
    }
}
