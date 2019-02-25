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
        Destroy,
        Error
    }

    public class Ship
    {
        public int size { get; set; }
       public Coords coords { get; private set; }

        public List<Coords> squares;
        private List<Coords> aliveSquares;

        public Direction direction { get; set; }



        public void setPosition(int x, int y, Direction d)
        {
            squares.Clear();
            aliveSquares.Clear();
            this.direction = d;
            this.coords = new Coords(x, y);
            if (d == Direction.Vertical)
            {
                for (int i = 0; i < size; i++)
                {
                    squares.Add(new Coords(x, y + i));
                    aliveSquares.Add(new Coords(x, y + i));
                }
            }
            else
            {
                for (int i = 0; i < size; i++)
                {
                    squares.Add(new Coords(x + i, y));
                    aliveSquares.Add(new Coords(x + i, y));
                }
            }
        }

        public Ship(int Size, int x, int y, Direction d)
        {
            size = Size;
            coords = new Coords (x, y);
            squares = new List<Coords>();
            aliveSquares = new List<Coords>();
            setPosition(x, y, d);
            
        }

        public HitResponse CheckHit(Coords shot)
        {
            HitResponse res = HitResponse.Miss;
            for(int i = 0; i < aliveSquares.Count; i++)
            {
                if (aliveSquares[i].Equals(shot))
                {
                    aliveSquares.RemoveAt(i);
                    if (aliveSquares.Count > 0)
                        res = HitResponse.Hit;
                    else
                        res = HitResponse.Destroy;
                }
            }
            return res;
        }

        public bool Collides(Ship s)
        {

            if (s.direction == this.direction)
            {
                if (this.direction == Direction.Horisontal)
                {
                    if (Math.Abs(s.coords.x - this.coords.x) > s.size || Math.Abs(s.coords.y - this.coords.y) >= 2)
                    {
                        return false;
                    }
                }
                else
                {
                    if (Math.Abs(s.coords.y - this.coords.y) > s.size || Math.Abs(s.coords.x - this.coords.x) >= 2)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                int xlast = s.squares[s.squares.Count - 1].x, ylast = s.squares[s.squares.Count - 1].y;
                int thisxlast = this.squares[this.squares.Count - 1].x, thisylast = this.squares[this.squares.Count - 1].y;

                bool first_first = Math.Abs(s.coords.x - this.coords.x) >= 2 || Math.Abs(s.coords.y - this.coords.y) >= 2;
                bool first_last = Math.Abs(s.coords.x - thisxlast) >= 2 || Math.Abs(s.coords.y - thisylast) >= 2;
                bool last_first = Math.Abs(xlast - this.coords.x) >= 2 || Math.Abs(ylast - this.coords.y) >= 2;
                bool last_last = Math.Abs(xlast - thisxlast) >= 2 || Math.Abs(ylast - thisylast) >= 2;
                if (first_first && first_last && last_first && last_last)
                    return false;
                return true;
            }
            //if (s.direction == Direction.Horisontal) {
            //    if (s.coords.Equals(this.coords) || s.coords.Equals(new Coords(this.coords.x - 1, this.coords.y - 1)) || s.coords.Equals(new Coords(this.coords.x - 1, this.coords.y + 1)) || s.coords.Equals(new Coords(this.coords.x - 1, this.coords.y)))
            //        return true;
            //    for (int i = 1; i < s.squares.Count - 1; i++)
            //    {
                    
            //    }
            //}

            //foreach(Coords c in s.squares)
            //{
            //    foreach(Coords c1 in this.squares)
            //    {
            //        bool res = false;
            //        res = c.Equals(c1)? 
            //    }
            //}
            
            return false;
        }
        
    }

  

    public class Coords
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
