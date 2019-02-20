using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace classes
{
    public enum Direction
    {
        Vertical, 
        Horisontal
    }
    public enum HitResponse
    {
        Miss, 
        Hit,
        Destroy
    }

    public class Ship
    {
        public int size { get; set; }
       public Coords coords { get; private set; }

        public List<Coords> squares; 

        public Direction direction { get; set; }

        public Ship(int Size, int x, int y, Direction d)
        {
            size = Size;
            coords = new Coords (x, y);
            squares = new List<Coords>();
            if(d == Direction.Vertical)
            {
                for(int i = 0; i < size; i++)
                {
                    squares[i] = new Coords(x, y + i);
                }
            }
            else{
                for (int i = 0; i < size; i++)
                {
                    squares[i] = new Coords(x + i, y);
                }
            }

        }

        public HitResponse CheckHit(Coords shot)
        {
            HitResponse res = HitResponse.Miss;
            for(int i = 0; i < squares.Count; i++)
            {
                if (squares[i].Equals(shot))
                {
                    squares.RemoveAt(i);
                    if (squares.Count > 0)
                        res = HitResponse.Hit;
                    else
                        res = HitResponse.Destroy;
                }
            }
            return res;
        }

    }

  

    public struct Coords
    {
        public Coords(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public bool Equals(Coords coords)
        {
            return coords.x == this.x && coords.y == this.y;
        }
        public int x;
        public int y;
    }
}
