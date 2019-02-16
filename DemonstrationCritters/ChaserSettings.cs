using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemonstrationCritters
{
    public partial class ChaserSettings : Form
    {
        Chaser critter;

        public ChaserSettings(Chaser chaser)
        {
            critter = chaser;

            InitializeComponent();

            trackBarNominalFeedingSpeed.Value = critter.EatSpeed;
            labelNominalSpeedShown.Text = trackBarNominalFeedingSpeed.Value.ToString();

            trackBarHeadForExitSpeed.Value = critter.HeadForExitSpeed;
            labelHeadForExitSpeedShown.Text = trackBarHeadForExitSpeed.Value.ToString();
        }

        private void TrackBarNominalFeedingSpeed_ValueChanged(object sender, EventArgs e)
        {
            labelNominalSpeedShown.Text = trackBarNominalFeedingSpeed.Value.ToString();
        }

        private void TrackBarHeadForExitSpeed_Scroll(object sender, EventArgs e)
        {
            labelHeadForExitSpeedShown.Text = trackBarHeadForExitSpeed.Value.ToString();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            critter.EatSpeed = int.Parse(labelNominalSpeedShown.Text);
            critter.HeadForExitSpeed = int.Parse(labelHeadForExitSpeedShown.Text);
            critter.SaveSettings();
            Dispose();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
