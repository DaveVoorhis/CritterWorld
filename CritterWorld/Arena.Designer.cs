namespace CritterWorld
{
    partial class Arena
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.spriteSurfaceMain = new SCG.TurboSprite.SpriteSurface(this.components);
            this.spriteEngineMain = new SCG.TurboSprite.SpriteEngine(this.components);
            this.labelFPS = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spriteSurfaceMain
            // 
            this.spriteSurfaceMain.Active = false;
            this.spriteSurfaceMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spriteSurfaceMain.AutoBlank = true;
            this.spriteSurfaceMain.AutoBlankColor = System.Drawing.Color.Black;
            this.spriteSurfaceMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.spriteSurfaceMain.CausesValidation = false;
            this.spriteSurfaceMain.DesiredFPS = 30;
            this.spriteSurfaceMain.Location = new System.Drawing.Point(0, 0);
            this.spriteSurfaceMain.Margin = new System.Windows.Forms.Padding(0);
            this.spriteSurfaceMain.Name = "spriteSurfaceMain";
            this.spriteSurfaceMain.OffsetX = 0;
            this.spriteSurfaceMain.OffsetY = 0;
            this.spriteSurfaceMain.Size = new System.Drawing.Size(813, 517);
            this.spriteSurfaceMain.TabIndex = 0;
            this.spriteSurfaceMain.Text = "spriteSurface1";
            this.spriteSurfaceMain.ThreadPriority = System.Threading.ThreadPriority.Normal;
            this.spriteSurfaceMain.UseVirtualSize = false;
            this.spriteSurfaceMain.VirtualHeight = 517;
            this.spriteSurfaceMain.VirtualSize = new System.Drawing.Size(813, 517);
            this.spriteSurfaceMain.VirtualWidth = 0;
            this.spriteSurfaceMain.WraparoundEdges = true;
            // 
            // spriteEngineMain
            // 
            this.spriteEngineMain.DetectCollisionSelf = true;
            this.spriteEngineMain.DetectCollisionTag = 0;
            this.spriteEngineMain.Priority = 1;
            this.spriteEngineMain.Surface = this.spriteSurfaceMain;
            // 
            // labelFPS
            // 
            this.labelFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFPS.AutoSize = true;
            this.labelFPS.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelFPS.ForeColor = System.Drawing.SystemColors.Window;
            this.labelFPS.Location = new System.Drawing.Point(746, 504);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(67, 13);
            this.labelFPS.TabIndex = 1;
            this.labelFPS.Text = "FPSFPSFPS";
            this.labelFPS.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // Arena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 517);
            this.Controls.Add(this.labelFPS);
            this.Controls.Add(this.spriteSurfaceMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Arena";
            this.Text = "CritterWorld";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SCG.TurboSprite.SpriteSurface spriteSurfaceMain;
        private SCG.TurboSprite.SpriteEngine spriteEngineMain;
        private System.Windows.Forms.Label labelFPS;
    }
}

