using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        private int tickCount = 0;

        private String TickShow()
        {
            if (tickCount++ > 5)
            {
                tickCount = 0;
            }
            return new string('.', tickCount);
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            new Level(arena, (Bitmap)Image.FromFile("Images/TerrainMasks/Background05.png"));
        }

        private void MenuStop_Click(object sender, EventArgs e)
        {
            arena.Clear();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public Critterworld()
        {
            InitializeComponent();

            System.Timers.Timer fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => Invoke(new Action(() => labelFPS.Text = arena.ActualFPS + " fps" + TickShow()));
            fpsDisplayTimer.Start();
        }
    }
}
