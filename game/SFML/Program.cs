using classes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFML
{
    class Program
    {
        Battlefield mainBF = new Battlefield();
        Battlefield opponentBF = new Battlefield();
        static void Main(string[] args)
        {
            var mode = new SFML.Window.VideoMode(800, 600);
            var window = new SFML.Graphics.RenderWindow(mode, "SFML works!");
            //window.KeyPressed += Window_KeyPressed;

            var circle = new SFML.Graphics.CircleShape(100f)
            {
                FillColor = SFML.Graphics.Color.Blue
            };

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();
                window.Draw(circle);

                // Finally, display the rendered frame on screen
                window.Display();
            }

        }
        private static void DrawBattlefield(Battlefield b, RenderWindow w)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    RectangleShape s = new RectangleShape(new Vector2f(10, 10));
                    switch (b.field[j, i])
                    {
                        //case 0:

                    }
                }
            }
        }
        private static void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
