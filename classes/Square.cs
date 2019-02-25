using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public class Square: Coords
    {
        public int flag { get; private set; }
        public Square(int x, int y, int f):base(x, y)
        {
            flag = f;
        }
    }
}
