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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.spriteSurfaceMain = new SCG.TurboSprite.SpriteSurface(this.components);
            this.spriteEngineMain = new SCG.TurboSprite.SpriteEngine(this.components);
            this.SuspendLayout();
            // 
            // spriteSurfaceMain
            // 
            this.spriteSurfaceMain.Active = false;
            this.spriteSurfaceMain.AutoBlank = true;
            this.spriteSurfaceMain.AutoBlankColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.spriteSurfaceMain.DesiredFPS = 30;
            this.spriteSurfaceMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spriteSurfaceMain.Location = new System.Drawing.Point(0, 0);
            this.spriteSurfaceMain.Margin = new System.Windows.Forms.Padding(0);
            this.spriteSurfaceMain.Name = "spriteSurfaceMain";
            this.spriteSurfaceMain.OffsetX = 0;
            this.spriteSurfaceMain.OffsetY = 0;
            this.spriteSurfaceMain.Size = new System.Drawing.Size(896, 629);
            this.spriteSurfaceMain.TabIndex = 0;
            this.spriteSurfaceMain.ThreadPriority = System.Threading.ThreadPriority.AboveNormal;
            this.spriteSurfaceMain.UseVirtualSize = false;
            this.spriteSurfaceMain.VirtualHeight = 629;
            this.spriteSurfaceMain.VirtualSize = new System.Drawing.Size(896, 629);
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
            // Arena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spriteSurfaceMain);
            this.Name = "Arena";
            this.Size = new System.Drawing.Size(896, 629);
            this.ResumeLayout(false);
        }

        #endregion

        private SCG.TurboSprite.SpriteSurface spriteSurfaceMain;
        private SCG.TurboSprite.SpriteEngine spriteEngineMain;
    }
}
