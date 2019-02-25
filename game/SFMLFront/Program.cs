using classes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SFMLFront
{
    class Program
    {
        const int PLAYER_START_X = 30;
        const int PLAYER_START_Y = 150;
        const int OPPONENT_START_X = 500;
        const int OPPONENT_START_Y = 150;
        
        const int SQUARE_SIZE = 30;
        const int SPACING_SIZE = 10;
        const int SIZE = 10;

        const int START_SHIPS = 30;

        static mqtt m = new mqtt();
        static RectangleShape[,] fieldMain = new RectangleShape[SIZE, SIZE];
        static RectangleShape[,] fieldOpponent = new RectangleShape[SIZE, SIZE];
        static List<RectangleShape> shipsToChoose = new List<RectangleShape>();
        static Battlefield mainBF = new Battlefield();
        static Battlefield opponentBF = new Battlefield();

        static bool choosing = true;
        static bool hovering = false;
        static Ship chosenShip = null;
        static void Main(string[] args)
        {
            var mode = new SFML.Window.VideoMode(1000, 600);
            var window = new SFML.Graphics.RenderWindow(mode, "SFML works!");
            window.SetFramerateLimit(60);
            m.connect();
            //window.MouseMoved += Window_MouseMoved;
            window.MouseButtonPressed += Window_MouseButtonPressed;
            m.gotHit += M_gotHit;
            //window.KeyPressed += Window_KeyPressed;

            //var circle = new SFML.Graphics.CircleShape(100f)
            //{
            //    FillColor = SFML.Graphics.Color.Blue
            //};


            // Start the game loop

            DrawBattlefield(mainBF, PLAYER_START_X, PLAYER_START_Y, fieldMain);
            DrawBattlefield(opponentBF, OPPONENT_START_X, OPPONENT_START_Y, fieldOpponent);
            while (window.IsOpen)
            {
                window.Clear();
                // Process events

                window.DispatchEvents();

                Field f = GetMouseField(Mouse.GetPosition(window));
                switch (f.Player)
                {
                    case 1:
                        fieldMain[f.X, f.Y].FillColor = Color.Red;
                        break;
                    case 2:
                        fieldOpponent[f.X, f.Y].FillColor = Color.Red;
                        break;
                    default:
                        DrawBattlefield(mainBF, PLAYER_START_X, PLAYER_START_Y, fieldMain);
                        DrawBattlefield(opponentBF, OPPONENT_START_X, OPPONENT_START_Y, fieldOpponent);
                        break;
                }
                //window.Draw(circle);
                DisplayBF(window, fieldOpponent, SIZE);
                DisplayBF(window, fieldMain, SIZE);
                DrawShips(window);
                if (chosenShip != null)
                {
                    DrawShip(Mouse.GetPosition(window), window, chosenShip, Color.Red);
                }
                // Finally, display the rendered frame on screen

                window.Display();
            }

        }

        

        private static void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("asld");
            
                
                RenderWindow w = (RenderWindow)sender;
                Field f = GetMouseField(Mouse.GetPosition(w));
                switch (f.Player)
                {
                    //player field
                    case 1:
                    if (chosenShip != null)
                    {
                        if (e.Button == Mouse.Button.Left)
                        {
                            chosenShip.setPosition(f.X, f.Y, chosenShip.direction);
                            mainBF.addShip(chosenShip);
                            chosenShip = null;
                        }
                        else if (e.Button == Mouse.Button.Right)
                        {
                            chosenShip.setPosition(chosenShip.coords.x, chosenShip.coords.y, chosenShip.direction == Direction.Horisontal ? Direction.Vertical : Direction.Horisontal);
                            
                        }
                    }
                        //mainBF.field[f.X, f.Y] = 2;
                        break;
                    case 2:             //opponent field
                        fieldOpponent[f.X, f.Y].FillColor = Color.Yellow;
                        break;
                    case 3:
                        chosenShip = new Ship(getShipFromSquare(f.X), 0, 0, Direction.Horisontal);
                        break;
                    default:
                        DrawBattlefield(mainBF, PLAYER_START_X, PLAYER_START_Y, fieldMain);
                        DrawBattlefield(opponentBF, OPPONENT_START_X, OPPONENT_START_Y, fieldOpponent);
                        break;
                
            }
        }

        private static void DrawShips(RenderWindow w)
        {
            for (int i = 1, j = 7, b = 0; i < 6; i++, j--)
            {
                int x = i * START_SHIPS + b * (SQUARE_SIZE + SPACING_SIZE);
                Ship s = new Ship(i, 0, 0, Direction.Horisontal);
                DrawShip(new Vector2i(x, START_SHIPS), w, s, Color.Cyan, true);
                b += i;
                //for (int y = 0; y < i; y++)
                //{
                //    shipsToChoose.Add(DrawSquare(x, START_SHIPS, w, Color.Yellow));
                //    x += SQUARE_SIZE + SPACING_SIZE;
                //    b++;
                //}
            }
        }

        private static void M_gotHit(object sender, EventArgs e)
        {
            opponentBF.addShot((Coords)sender);
            //Console.WriteLine(((Coords)sender).x);
            DrawBattlefield(opponentBF, OPPONENT_START_X, OPPONENT_START_Y, fieldOpponent);
        }

        

        private static void Window_MouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {
            var window = (RenderWindow)sender;
            int xt = e.X;
            int yt = e.Y;
           

            if (mouseOverSquare(PLAYER_START_X, PLAYER_START_Y, e.X, e.Y))
            {
                //Console.WriteLine("IN");
                xt -= PLAYER_START_X;
                yt -= PLAYER_START_Y;
                int yf = yt / (SQUARE_SIZE + SPACING_SIZE);
                int xf = xt / (SQUARE_SIZE + SPACING_SIZE);
                if (mainBF.field[xf, yf] == 0)
                    fieldMain[xf, yf].FillColor = Color.Red;

            }
            else if(mouseOverSquare(OPPONENT_START_X, OPPONENT_START_Y, e.X, e.Y))
            {
                xt -= OPPONENT_START_X;
                yt -= OPPONENT_START_Y;
                int yf = yt / (SQUARE_SIZE + SPACING_SIZE);
                int xf = xt / (SQUARE_SIZE + SPACING_SIZE);
                if (opponentBF.field[xf, yf] == 0)
                    fieldOpponent[xf, yf].FillColor = Color.Red;
            }
            else
            {
                //Console.WriteLine("OUT");
                DrawBattlefield(mainBF, PLAYER_START_X, PLAYER_START_Y, fieldMain);
                DrawBattlefield(opponentBF, OPPONENT_START_X, OPPONENT_START_Y, fieldOpponent);
            }
        }
        
        
        private static int getShipFromSquare(int square)
        {
            for(int i = 1, j = 0; i <= 5; i++)
            {
                for (int y = 0; y < i; y++)
                {
                    j++;
                    if (j == square)
                        return i;
                }
            }
            return 0;
        }
        private static Field GetMouseField(Vector2i mousePos)
        {
            Field r = new Field(mousePos.X, mousePos.Y, 0);
            if (mouseOverSquare(PLAYER_START_X, PLAYER_START_Y, mousePos.X, mousePos.Y))
            {
                //Console.WriteLine("IN");
                int xt = mousePos.X - PLAYER_START_X;
                int yt = mousePos.Y - PLAYER_START_Y;
                int yf = yt / (SQUARE_SIZE + SPACING_SIZE);
                int xf = xt / (SQUARE_SIZE + SPACING_SIZE);
                return new Field(xf, yf, 1);
            }
            else if (mouseOverSquare(OPPONENT_START_X, OPPONENT_START_Y, mousePos.X, mousePos.Y))
            {
                int xt = mousePos.X - OPPONENT_START_X;
                int yt = mousePos.Y - OPPONENT_START_Y;
                int yf = yt / (SQUARE_SIZE + SPACING_SIZE);
                int xf = xt / (SQUARE_SIZE + SPACING_SIZE);
                return new Field(xf, yf, 2);
            }
            else if (mousePos.X > 0 && mousePos.Y > START_SHIPS && mousePos.Y < START_SHIPS + SQUARE_SIZE)
            {
                int count = 0;
                foreach(RectangleShape s in shipsToChoose)
                {
                    count++;
                    if (mousePos.X > s.Position.X && mousePos.X < s.Position.X + SQUARE_SIZE && mousePos.Y > s.Position.Y && mousePos.Y < s.Position.Y + SQUARE_SIZE)
                    {
                        return new Field(count, 0, 3);
                    }
                    
                }
                return new Field(1, 1, 0);
            }
            else
            {
                //Console.WriteLine("OUT");
                return new Field(1, 1, 0);
            }
        }
        //private static bool coordsIn(Vector2i c1, Vector2i c2)
        //{
        //    return (c2.X > )
        //}
        private static RectangleShape DrawSquare(int x, int y, RenderWindow w, Color color,  int size = SQUARE_SIZE)
        {
            RectangleShape s = new RectangleShape(new Vector2f(size, size));
            s.Position = new Vector2f((float)x, (float)y);
            s.FillColor = color;
            //Console.WriteLine(x + "--" + y);

            w.Draw(s);
            s.Dispose();
            return s;
        }
        private static void DrawSquare(RectangleShape s, RenderWindow w)
        {
            w.Draw(s);
        }
        
        private static bool mouseOverSquare(int startx, int starty, int x, int y)
        {
            int xend = startx + SQUARE_SIZE * 10 + 10 * SPACING_SIZE;
            int yend = starty + SQUARE_SIZE * 10 + 10 * SPACING_SIZE;
            if (x > startx && y > starty && x < xend && y < yend)
            {

                int xt = x - startx;
                int xf = xt / (SQUARE_SIZE + SPACING_SIZE);
                int xmax = (xf * SQUARE_SIZE + SPACING_SIZE * xf) + SQUARE_SIZE;
                int yt = y - starty;
                int yf = yt / (SQUARE_SIZE + SPACING_SIZE);
                int ymax = (yf * SQUARE_SIZE + SPACING_SIZE * yf) + SQUARE_SIZE;
                if (xt > xmax || yt > ymax)
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
            //return true;
        }
        private static void DisplayBF(RenderWindow w, RectangleShape[,] shape, int size)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    DrawSquare(shape[j, i], w);
                }
            }
        }
        private static void DrawBattlefield(Battlefield b, int startX, int startY, RectangleShape[,] shape)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //Console.WriteLine("Making shape " + i*j);
                    RectangleShape s = new RectangleShape(new Vector2f(SQUARE_SIZE, SQUARE_SIZE));
                    switch (b.field[j, i])
                    {
                        case 0:
                            s.FillColor = Color.Blue;

                            break;
                        case 1:
                            s.FillColor = Color.Green;
                            break;
                        case 2:
                            s.FillColor = Color.Yellow;
                            break;
                        default:
                            s.FillColor = Color.Black;
                            break;
                    }
                    s.Position = new Vector2f(startX + j * SQUARE_SIZE + j * SPACING_SIZE, startY + i * SQUARE_SIZE + i * SPACING_SIZE);
                    shape[j, i] = new RectangleShape(s);
                }
            }
        }

        private static void DrawShip(Vector2i startCoord, RenderWindow w, Ship s, Color c, bool shipToChoose = false)
        {
            
            for(int i = 0; i < s.size; i++)
            {
                RectangleShape shape = new RectangleShape(new Vector2f(SQUARE_SIZE, SQUARE_SIZE));
                int add = (SQUARE_SIZE + SPACING_SIZE) * i;
                
                if(s.direction == Direction.Horisontal)
                {
                    shape.Position = new Vector2f(startCoord.X + add, startCoord.Y);
                }
                else
                {
                    shape.Position = new Vector2f(startCoord.X, startCoord.Y + add);
                }

                w.Draw(shape);
                if (shipToChoose)
                    shipsToChoose.Add(shape);
                
            }
        }

        private static void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
    

