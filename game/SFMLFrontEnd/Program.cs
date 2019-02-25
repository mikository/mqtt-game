using SFML.Graphics;
using SFML.Window;
using System;

namespace SFMLFrontEnd
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(200, 200), "SFML");
            CircleShape shape = new CircleShape(100);
            shape.FillColor = Color.Red;
            window.Closed += Window_Closed;

            while (window.IsOpen)
            {
                window.Clear();
                window.Draw(shape);
                window.Display();
            }

        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
