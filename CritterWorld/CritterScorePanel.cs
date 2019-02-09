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
        private SpriteEngine spriteEngine;
        private PolygonSprite critterImage;
        private Critter critter;

        private void UpdateScore(int currentScore, int overallScore)
        {
            labelScore.Text = "Score: " + currentScore + "/" + overallScore;
        }

        private void UpdateHealthAndEnergy(float health, float energy)
        {
            progressBarHealth.Value = (int)health;
            progressBarHealth.ForeColor = (progressBarHealth.Value < 25) ? Color.Red : Color.Green;
            progressBarEnergy.Value = (int)energy;
            progressBarEnergy.ForeColor = (progressBarEnergy.Value < 25) ? Color.Red : Color.Green;
        }

        private void MakeProgressBarsVisible(bool visible)
        {
            labelHealth.Visible = visible;
            labelEnergy.Visible = visible;
            progressBarHealth.Visible = visible;
            progressBarEnergy.Visible = visible;
        }

        public CritterScorePanel()
        {
            InitializeComponent();

            // Click on anything to highlight critter
            Click += (e, evt) => critter.ShowShockwave();
            components.Components.Cast<Control>().ToList().ForEach(control => control.Click += (e, evt) => critter.ShowShockwave());

            spriteEngine = new SpriteEngine
            {
                Surface = spriteSurfaceCritter
            };

            SetCritter(null);
        }

        public void SetCritter(Critter theCritter)
        {
            critter = theCritter;
            if (critter == null)
            {
                MakeProgressBarsVisible(false);
                labelNumber.Text = "000";
                labelNumber.Visible = false;
                labelName.Text = "";
                labelName.Visible = false;
                labelEscaped.Visible = false;
                labelDead.Visible = false;
                UpdateScore(0, 0);
                labelScore.Visible = false;
                UpdateHealthAndEnergy(0, 0);
                if (critterImage != null)
                {
                    spriteEngine.RemoveSprite(critterImage);
                }
            }
            else
            {
                labelNumber.Visible = true;
                labelName.Visible = true;
                labelScore.Visible = true;

                critterImage = new PolygonSprite(critter.Model)
                {
                    Color = critter.Color,
                    Position = new Point(spriteSurfaceCritter.Width / 2, spriteSurfaceCritter.Height / 2)
                };
                spriteEngine.AddSprite(critterImage);

                labelNumber.Text = critter.Number.ToString();
                labelName.Text = critter.NameAndAuthor;

                MakeProgressBarsVisible(true);

                spriteSurfaceCritter.Active = true;
            }
        }

        public void CritterUpdate()
        {
            if (critter == null)
            {
                return;
            }
            UpdateScore(critter.CurrentScore, critter.OverallScore);
            UpdateHealthAndEnergy(critter.Health, critter.Energy);
            if (critter.IsEscaped)
            {
                labelEscaped.Visible = true;
                UpdateScore(critter.CurrentScore, critter.OverallScore);
                MakeProgressBarsVisible(false);
            }
            if (critter.IsDead)
            {
                labelDead.Visible = true;
                labelScore.Text = critter.DeadReason;
                MakeProgressBarsVisible(false);
            }
        }

    }
}
