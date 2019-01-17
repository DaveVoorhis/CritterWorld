namespace CritterWorld
{
    partial class CritterScorePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CritterScorePanel));
            this.labelScore = new System.Windows.Forms.Label();
            this.progressBarEnergy = new System.Windows.Forms.ProgressBar();
            this.progressBarHealth = new System.Windows.Forms.ProgressBar();
            this.labelName = new System.Windows.Forms.Label();
            this.labelHealth = new System.Windows.Forms.Label();
            this.labelEnergy = new System.Windows.Forms.Label();
            this.labelEscaped = new System.Windows.Forms.Label();
            this.labelDead = new System.Windows.Forms.Label();
            this.spriteSurfaceCritter = new SCG.TurboSprite.SpriteSurface(this.components);
            this.labelNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelScore
            // 
            this.labelScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScore.Location = new System.Drawing.Point(75, 0);
            this.labelScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(88, 16);
            this.labelScore.TabIndex = 1;
            this.labelScore.Text = "Score:";
            // 
            // progressBarEnergy
            // 
            this.progressBarEnergy.Location = new System.Drawing.Point(264, 7);
            this.progressBarEnergy.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarEnergy.Name = "progressBarEnergy";
            this.progressBarEnergy.Size = new System.Drawing.Size(34, 7);
            this.progressBarEnergy.TabIndex = 6;
            // 
            // progressBarHealth
            // 
            this.progressBarHealth.Location = new System.Drawing.Point(264, 17);
            this.progressBarHealth.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarHealth.Name = "progressBarHealth";
            this.progressBarHealth.Size = new System.Drawing.Size(34, 7);
            this.progressBarHealth.TabIndex = 7;
            // 
            // labelName
            // 
            this.labelName.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(48, 17);
            this.labelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(186, 21);
            this.labelName.TabIndex = 8;
            this.labelName.Text = "Name";
            // 
            // labelHealth
            // 
            this.labelHealth.Image = ((System.Drawing.Image)(resources.GetObject("labelHealth.Image")));
            this.labelHealth.Location = new System.Drawing.Point(246, 16);
            this.labelHealth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelHealth.Name = "labelHealth";
            this.labelHealth.Size = new System.Drawing.Size(16, 16);
            this.labelHealth.TabIndex = 5;
            // 
            // labelEnergy
            // 
            this.labelEnergy.Image = ((System.Drawing.Image)(resources.GetObject("labelEnergy.Image")));
            this.labelEnergy.Location = new System.Drawing.Point(246, 2);
            this.labelEnergy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEnergy.Name = "labelEnergy";
            this.labelEnergy.Size = new System.Drawing.Size(16, 16);
            this.labelEnergy.TabIndex = 4;
            // 
            // labelEscaped
            // 
            this.labelEscaped.Image = ((System.Drawing.Image)(resources.GetObject("labelEscaped.Image")));
            this.labelEscaped.Location = new System.Drawing.Point(187, 0);
            this.labelEscaped.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEscaped.Name = "labelEscaped";
            this.labelEscaped.Size = new System.Drawing.Size(16, 16);
            this.labelEscaped.TabIndex = 3;
            this.labelEscaped.Visible = false;
            // 
            // labelDead
            // 
            this.labelDead.Image = ((System.Drawing.Image)(resources.GetObject("labelDead.Image")));
            this.labelDead.Location = new System.Drawing.Point(167, 0);
            this.labelDead.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDead.Name = "labelDead";
            this.labelDead.Size = new System.Drawing.Size(16, 16);
            this.labelDead.TabIndex = 10;
            this.labelDead.Visible = false;
            // 
            // spriteSurfaceCritter
            // 
            this.spriteSurfaceCritter.Active = false;
            this.spriteSurfaceCritter.AutoBlank = true;
            this.spriteSurfaceCritter.AutoBlankColor = System.Drawing.Color.Black;
            this.spriteSurfaceCritter.DesiredFPS = 10;
            this.spriteSurfaceCritter.Location = new System.Drawing.Point(2, -1);
            this.spriteSurfaceCritter.Margin = new System.Windows.Forms.Padding(2);
            this.spriteSurfaceCritter.Name = "spriteSurfaceCritter";
            this.spriteSurfaceCritter.OffsetX = 0;
            this.spriteSurfaceCritter.OffsetY = 0;
            this.spriteSurfaceCritter.Size = new System.Drawing.Size(38, 39);
            this.spriteSurfaceCritter.TabIndex = 0;
            this.spriteSurfaceCritter.ThreadPriority = System.Threading.ThreadPriority.Normal;
            this.spriteSurfaceCritter.UseVirtualSize = false;
            this.spriteSurfaceCritter.VirtualHeight = 39;
            this.spriteSurfaceCritter.VirtualSize = new System.Drawing.Size(38, 39);
            this.spriteSurfaceCritter.VirtualWidth = 0;
            this.spriteSurfaceCritter.WraparoundEdges = false;
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNumber.Location = new System.Drawing.Point(45, 0);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(27, 15);
            this.labelNumber.TabIndex = 11;
            this.labelNumber.Text = "999";
            // 
            // CritterScorePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.labelDead);
            this.Controls.Add(this.labelEscaped);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.progressBarHealth);
            this.Controls.Add(this.progressBarEnergy);
            this.Controls.Add(this.labelHealth);
            this.Controls.Add(this.labelEnergy);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.spriteSurfaceCritter);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CritterScorePanel";
            this.Size = new System.Drawing.Size(300, 39);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SCG.TurboSprite.SpriteSurface spriteSurfaceCritter;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelEscaped;
        private System.Windows.Forms.Label labelEnergy;
        private System.Windows.Forms.Label labelHealth;
        private System.Windows.Forms.ProgressBar progressBarEnergy;
        private System.Windows.Forms.ProgressBar progressBarHealth;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelDead;
        private System.Windows.Forms.Label labelNumber;
    }
}
