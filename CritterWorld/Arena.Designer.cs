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
            this.spriteEngineMain = new SCG.TurboSprite.SpriteEngineDestination(this.components);
            this.SuspendLayout();
            // 
            // spriteSurface1
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
            this.spriteSurfaceMain.Name = "spriteSurface1";
            this.spriteSurfaceMain.OffsetX = 0;
            this.spriteSurfaceMain.OffsetY = 0;
            this.spriteSurfaceMain.Size = new System.Drawing.Size(1626, 994);
            this.spriteSurfaceMain.TabIndex = 0;
            this.spriteSurfaceMain.Text = "spriteSurface1";
            this.spriteSurfaceMain.ThreadPriority = System.Threading.ThreadPriority.Normal;
            this.spriteSurfaceMain.UseVirtualSize = false;
            this.spriteSurfaceMain.VirtualHeight = 994;
            this.spriteSurfaceMain.VirtualSize = new System.Drawing.Size(1626, 994);
            this.spriteSurfaceMain.VirtualWidth = 0;
            this.spriteSurfaceMain.WraparoundEdges = true;
            // 
            // spriteEngine1
            // 
            this.spriteEngineMain.DetectCollisionSelf = true;
            this.spriteEngineMain.DetectCollisionTag = 0;
            this.spriteEngineMain.Priority = 1;
            this.spriteEngineMain.Surface = this.spriteSurfaceMain;
            // 
            // Arena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1626, 994);
            this.Controls.Add(this.spriteSurfaceMain);
            this.Name = "Arena";
            this.Text = "CritterWorld";
            this.ResumeLayout(false);

        }

        #endregion

        private SCG.TurboSprite.SpriteSurface spriteSurfaceMain;
        private SCG.TurboSprite.SpriteEngineDestination spriteEngineMain;
    }
}

