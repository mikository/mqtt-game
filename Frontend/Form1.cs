using classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Frontend
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Battlefield battlefieldMain = new Battlefield();
        public Battlefield battleFieldSecond = new Battlefield();


        public void drawHits()
        {
            List<Coords> undrawed = new List<Coords>();
                undrawed.AddRange(battleFieldSecond.shots);
            foreach (PictureBox p in this.panel2.Controls)
            {
                Coords c = (Coords)p.Tag;
                foreach (Coords co in undrawed)
                {
                    if (co.Equals(c))
                    {
                        p.BackColor = Color.Red;
                        undrawed.Remove(co);
                        break;
                    }
                }
            }
        }
        public void clearShips()
        {
            foreach (Control ctl in this.panel2.Controls)
            {
                if (ctl is PictureBox)
                {
                    ctl.BackColor = Color.White;
                }
            }
        }
        mqtt m = new mqtt();
        private void Form1_Load(object sender, EventArgs e)
        {
            
            m.gotHit += M_gotHit;
            m.connect();
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
                    coords1.x = z;
                    coords1.y = i;
                    _pc.Tag = coords1;
                   
                    panel1.Controls.Add(_pc);
                }
                setShips();
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
                    coords1.x = z;
                    coords1.y = i;
                    _pc.Tag = coords1;
                    panel2.Controls.Add(_pc);
                }
            }
            drawShips();
        }
        delegate void addShipCallBack(Coords c);
        private void addShot(Coords c)
        {
            if (this.InvokeRequired)
            {
                addShipCallBack ca = new addShipCallBack(addShot);
                this.Invoke(ca, new object[] { c });
                
            }
            else
            {
                clearShips();
                drawHits();
            }
        }

        private void M_gotHit(object sender, EventArgs e)
        {
            battleFieldSecond.addShot((Coords)sender);
            clearShips();
            drawHits();
            //addShot((Coords)sender);

        }

        public void setShips()
        {
            foreach (Control ctl in this.panel1.Controls)
            {
                if (ctl is PictureBox)
                {
                    ctl.Click += new EventHandler(pictureboxes_Click);
                }
            }
        }

        void pictureboxes_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pc)
            {
                pc.BackColor = Color.Green;
            }

        }

        public void drawShips()
        {
            List<Coords> undrawed = battlefieldMain.GetCoords();
            foreach(PictureBox p in this.panel1.Controls)
            {
                Coords c = (Coords)p.Tag;
                foreach(Coords co in undrawed)
                {
                    if (co.Equals(c))
                    {
                        p.BackColor = Color.Red;
                        undrawed.Remove(co);
                        break;
                    }
                }
            }
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            m.close();
        }
    }
}
