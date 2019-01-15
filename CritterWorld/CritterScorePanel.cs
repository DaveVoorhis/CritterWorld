using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class CritterScorePanel : UserControl, IScoreDisplay
    {
        private int currentScore;
        private int overallScore;
        private int critterNumber;

        private void UpdateScore()
        {
            Invoke(new Action(() => labelScore.Text = currentScore + "/" + overallScore));
        }

        public CritterScorePanel()
        {
            InitializeComponent();
        }

        int IScoreDisplay.CritterNumber { set => critterNumber = value; }

        string IScoreDisplay.Name { set => Invoke(new Action(() => labelName.Text = value)); }

        string IScoreDisplay.Author { set => Invoke(new Action(() => labelAuthor.Text = value)); }

        bool IScoreDisplay.Escaped { set => Invoke(new Action(() => labelEscaped.Visible = value)); }

        int IScoreDisplay.Energy { set => Invoke(new Action(() => progressBarEnergy.Value = value)); }

        bool IScoreDisplay.Killed { set => Invoke(new Action(() => labelDead.Visible = value)); }

        int IScoreDisplay.Health { set => Invoke(new Action(() => progressBarHealth.Value = value)); }

        int IScoreDisplay.CurrentScore
        {
            set
            {
                currentScore = value;
                UpdateScore();
            }
        }

        int IScoreDisplay.OverallScore {
            set
            {
                overallScore = value;
                UpdateScore();
            }
        }

    }
}
