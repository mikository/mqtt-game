using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public struct Coords
        {
            public int x, y;

            public Coords(int p1, int p2)
            {
                x = p1;
                y = p2;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize:   
            Coords coords1 = new Coords(0, 0);

            //your side 
            for (int i = 0; i < 10; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    PictureBox _pc = new PictureBox();
                    _pc.Size = new System.Drawing.Size(30, 30);
                    _pc.Location = new System.Drawing.Point(coords1.y + z * 40, coords1.x + i * 40);
                    _pc.BackColor = Color.White; 
                    _pc.Tag = coords1; 
                    panel1.Controls.Add(_pc);
                }
            }

            //your opponent
            for (int i = 0; i < 10; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    PictureBox _pc = new PictureBox();
                    _pc.Size = new System.Drawing.Size(30, 30);
                    _pc.Location = new System.Drawing.Point(coords1.y + z * 40, coords1.x + i * 40);
                    _pc.BackColor = Color.White;
                    _pc.Tag = coords1;
                    panel2.Controls.Add(_pc);
                }
            }

        }

        public void setShips()
        {

        }
    }
}
