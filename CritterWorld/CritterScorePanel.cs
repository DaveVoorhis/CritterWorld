using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCG.TurboSprite;

namespace CritterWorld
{
    public partial class CritterScorePanel : UserControl
    {
        private Timer timer = null;

        private void UpdateScore(int currentScore, int overallScore)
        {
            labelScore.Text = "Score: " + currentScore + "/" + overallScore;
        }

        private void MakeProgressBarsInvisible()
        {
            labelHealth.Visible = false;
            labelEnergy.Visible = false;
            progressBarHealth.Visible = false;
            progressBarEnergy.Visible = false;
        }

        public CritterScorePanel(Critter critter)
        {
            InitializeComponent();

            SpriteEngine spriteEngine = new SpriteEngine
            {
                Surface = spriteSurfaceCritter
            };

            PolygonSprite critterImage = new PolygonSprite(critter.Model)
            {
                Color = critter.Color,
                Position = new Point(spriteSurfaceCritter.Width / 2, spriteSurfaceCritter.Height / 2)
            };
            spriteEngine.AddSprite(critterImage);

            labelNumber.Text = critter.Number.ToString();
            labelName.Text = critter.NameAndAuthor;
 
            timer = new Timer
            {
                Interval = 500
            };
            timer.Tick += (e, evt) =>
            {
                UpdateScore(critter.CurrentScore, critter.OverallScore);
                progressBarHealth.Value = (int)critter.Health;
                progressBarHealth.ForeColor = (progressBarHealth.Value < 25) ? Color.Red : Color.Green;
                progressBarEnergy.Value = (int)critter.Energy;
                progressBarEnergy.ForeColor = (progressBarEnergy.Value < 25) ? Color.Red : Color.Green;
                if (critter.IsEscaped)
                {
                    labelEscaped.Visible = true;
                    UpdateScore(critter.CurrentScore, critter.OverallScore);
                    MakeProgressBarsInvisible();
                }
                if (critter.IsDead)
                {
                    labelDead.Visible = true;
                    labelScore.Text = critter.DeadReason;
                    MakeProgressBarsInvisible();
                }
            };
            timer.Start();
        }

        public void Shutdown()
        {
            timer.Stop();
            foreach (Control control in Controls)
            {
                control.Dispose();
            }
        }
    }
}
