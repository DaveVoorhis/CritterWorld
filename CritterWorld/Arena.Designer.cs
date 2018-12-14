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
            this.spriteSurface1 = new SCG.TurboSprite.SpriteSurface(this.components);
            this.spriteEngine1 = new SCG.TurboSprite.SpriteEngineDestination(this.components);
            this.SuspendLayout();
            // 
            // spriteSurface1
            // 
            this.spriteSurface1.Active = false;
            this.spriteSurface1.AutoBlank = true;
            this.spriteSurface1.AutoBlankColor = System.Drawing.Color.Black;
            this.spriteSurface1.DesiredFPS = 30;
            this.spriteSurface1.Location = new System.Drawing.Point(40, 40);
            this.spriteSurface1.Name = "spriteSurface1";
            this.spriteSurface1.OffsetX = 0;
            this.spriteSurface1.OffsetY = 0;
            this.spriteSurface1.Size = new System.Drawing.Size(1555, 927);
            this.spriteSurface1.TabIndex = 0;
            this.spriteSurface1.Text = "spriteSurface1";
            this.spriteSurface1.ThreadPriority = System.Threading.ThreadPriority.Normal;
            this.spriteSurface1.UseVirtualSize = false;
            this.spriteSurface1.VirtualHeight = 0;
            this.spriteSurface1.VirtualSize = new System.Drawing.Size(1555, 927);
            this.spriteSurface1.VirtualWidth = 0;
            this.spriteSurface1.WraparoundEdges = false;
            // 
            // spriteEngine1
            // 
            this.spriteEngine1.DetectCollisionSelf = true;
            this.spriteEngine1.DetectCollisionTag = 0;
            this.spriteEngine1.Priority = 1;
            this.spriteEngine1.Surface = this.spriteSurface1;
            // 
            // Arena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1626, 994);
            this.Controls.Add(this.spriteSurface1);
            this.Name = "Arena";
            this.Text = "CritterWorld";
            this.ResumeLayout(false);

        }

        #endregion

        private SCG.TurboSprite.SpriteSurface spriteSurface1;
        private SCG.TurboSprite.SpriteEngineDestination spriteEngine1;
    }
}

