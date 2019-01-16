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
            labelScore.Text = currentScore + "/" + overallScore;
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
            labelName.Text = critter.Name + " by " + critter.Author;
 
            timer = new Timer
            {
                Interval = 500
            };
            timer.Tick += (e, evt) =>
            {
                UpdateScore(critter.CurrentScore, critter.OverallScore);
                progressBarHealth.Value = critter.Health;
                progressBarEnergy.Value = critter.Energy;
                if (critter.IsEscaped)
                {
                    labelEscaped.Visible = true;
                    UpdateScore(critter.CurrentScore, critter.OverallScore);
                    timer.Stop();
                }
                if (critter.IsDead)
                {
                    labelDead.Visible = true;
                    timer.Stop();
                }
            };
            timer.Start();
        }

        public void Shutdown()
        {
            timer.Stop();
        }
    }
}
