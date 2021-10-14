using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JeuDeLaViePDF
{
    public partial class Form1 : Form
    {
        private Game game;
        public int n;

        public Form1()
        {
            n = 40;
            InitializeComponent(n);

            game = new Game(n); 
            
            Timer MyTimer = new Timer();
            MyTimer.Interval = (600);
            MyTimer.Tick += new EventHandler(MyTimer_Tick);
            MyTimer.Start();

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            game.Paint(e.Graphics);
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            game.grid.UpdateGrid();
            Refresh();
        }
    }
}
