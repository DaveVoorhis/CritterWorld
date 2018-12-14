namespace TurboSpriteTest
{
    partial class TurboSpriteTestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TurboSpriteTestForm));
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnAddSprite = new System.Windows.Forms.Button();
            this.lblFPS = new System.Windows.Forms.Label();
            this.lblSprites = new System.Windows.Forms.Label();
            this.picGlyph = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPolygon = new System.Windows.Forms.Button();
            this.btnPiece = new System.Windows.Forms.Button();
            this.pnlColor = new System.Windows.Forms.Panel();
            this.dlgColor = new System.Windows.Forms.ColorDialog();
            this.btnParticle = new System.Windows.Forms.Button();
            this.btnShockWave = new System.Windows.Forms.Button();
            this.surface = new SCG.TurboSprite.SquareGridSpriteSurface(this.components);
            this.engineDest = new SCG.TurboSprite.SpriteEngineDestination(this.components);
            this.engineStars = new SCG.TurboSprite.SpriteEngine(this.components);
            this.pieces = new SCG.TurboSprite.GamePieceBitmapFactory(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picGlyph)).BeginInit();
            this.SuspendLayout();
            // 
            // btnActivate
            // 
            this.btnActivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnActivate.Location = new System.Drawing.Point(8, 391);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(75, 23);
            this.btnActivate.TabIndex = 1;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnAddSprite
            // 
            this.btnAddSprite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddSprite.Location = new System.Drawing.Point(229, 391);
            this.btnAddSprite.Name = "btnAddSprite";
            this.btnAddSprite.Size = new System.Drawing.Size(75, 23);
            this.btnAddSprite.TabIndex = 2;
            this.btnAddSprite.Text = "BitmapSprite";
            this.btnAddSprite.UseVisualStyleBackColor = true;
            this.btnAddSprite.Click += new System.EventHandler(this.btnAddSprite_Click);
            // 
            // lblFPS
            // 
            this.lblFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFPS.AutoSize = true;
            this.lblFPS.Location = new System.Drawing.Point(89, 399);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(27, 13);
            this.lblFPS.TabIndex = 3;
            this.lblFPS.Text = "FPS";
            // 
            // lblSprites
            // 
            this.lblSprites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSprites.AutoSize = true;
            this.lblSprites.Location = new System.Drawing.Point(145, 399);
            this.lblSprites.Name = "lblSprites";
            this.lblSprites.Size = new System.Drawing.Size(39, 13);
            this.lblSprites.TabIndex = 4;
            this.lblSprites.Text = "Sprites";
            // 
            // picGlyph
            // 
            this.picGlyph.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picGlyph.Image = global::TurboSpriteTest.Properties.Resources.Glyph;
            this.picGlyph.Location = new System.Drawing.Point(718, 391);
            this.picGlyph.Name = "picGlyph";
            this.picGlyph.Size = new System.Drawing.Size(32, 32);
            this.picGlyph.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picGlyph.TabIndex = 5;
            this.picGlyph.TabStop = false;
            this.picGlyph.Visible = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(402, 391);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "StarField";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPolygon
            // 
            this.btnPolygon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPolygon.Location = new System.Drawing.Point(310, 391);
            this.btnPolygon.Name = "btnPolygon";
            this.btnPolygon.Size = new System.Drawing.Size(86, 23);
            this.btnPolygon.TabIndex = 9;
            this.btnPolygon.Text = "PolygonSprite";
            this.btnPolygon.UseVisualStyleBackColor = true;
            this.btnPolygon.Click += new System.EventHandler(this.btnPolygon_Click);
            // 
            // btnPiece
            // 
            this.btnPiece.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPiece.Location = new System.Drawing.Point(483, 391);
            this.btnPiece.Name = "btnPiece";
            this.btnPiece.Size = new System.Drawing.Size(75, 23);
            this.btnPiece.TabIndex = 10;
            this.btnPiece.Text = "GamePiece";
            this.btnPiece.UseVisualStyleBackColor = true;
            this.btnPiece.Click += new System.EventHandler(this.btnPiece_Click);
            // 
            // pnlColor
            // 
            this.pnlColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlColor.BackColor = System.Drawing.Color.Red;
            this.pnlColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlColor.Location = new System.Drawing.Point(564, 392);
            this.pnlColor.Name = "pnlColor";
            this.pnlColor.Size = new System.Drawing.Size(25, 21);
            this.pnlColor.TabIndex = 11;
            this.pnlColor.Click += new System.EventHandler(this.pnlColor_Click);
            // 
            // dlgColor
            // 
            this.dlgColor.Color = System.Drawing.Color.Red;
            // 
            // btnParticle
            // 
            this.btnParticle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnParticle.Location = new System.Drawing.Point(595, 391);
            this.btnParticle.Name = "btnParticle";
            this.btnParticle.Size = new System.Drawing.Size(75, 23);
            this.btnParticle.TabIndex = 12;
            this.btnParticle.Text = "Particle";
            this.btnParticle.UseVisualStyleBackColor = true;
            this.btnParticle.Click += new System.EventHandler(this.btnParticle_Click);
            // 
            // btnShockWave
            // 
            this.btnShockWave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShockWave.Location = new System.Drawing.Point(675, 391);
            this.btnShockWave.Name = "btnShockWave";
            this.btnShockWave.Size = new System.Drawing.Size(75, 23);
            this.btnShockWave.TabIndex = 13;
            this.btnShockWave.Text = "ShockWave";
            this.btnShockWave.UseVisualStyleBackColor = true;
            this.btnShockWave.Click += new System.EventHandler(this.btnShockWave_Click);
            // 
            // surface
            // 
            this.surface.Active = false;
            this.surface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.surface.AutoBlank = true;
            this.surface.AutoBlankColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
            this.surface.BackColor = System.Drawing.Color.Red;
            this.surface.CellHeight = 40;
            this.surface.CellsX = 100;
            this.surface.CellsY = 100;
            this.surface.CellWidth = 40;
            this.surface.CursorColor = System.Drawing.Color.Empty;
            this.surface.CursorVisible = false;
            this.surface.CursorWidth = 4;
            this.surface.CursorX = 0;
            this.surface.CursorY = 0;
            this.surface.DesiredFPS = 50;
            this.surface.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.surface.GridVisible = true;
            this.surface.Location = new System.Drawing.Point(8, 8);
            this.surface.Name = "surface";
            this.surface.OffsetCellX = 0;
            this.surface.OffsetCellY = 0;
            this.surface.OffsetX = 0;
            this.surface.OffsetY = 0;
            this.surface.SelectionBand = true;
            this.surface.SelectionBandColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.surface.Size = new System.Drawing.Size(742, 375);
            this.surface.TabIndex = 0;
            this.surface.Text = "spriteSurface1";
            this.surface.ThreadPriority = System.Threading.ThreadPriority.Normal;
            this.surface.UseVirtualSize = false;
            this.surface.VirtualHeight = 375;
            this.surface.VirtualSize = new System.Drawing.Size(742, 375);
            this.surface.VirtualWidth = 4000;
            this.surface.WraparoundEdges = true;
            this.surface.BeforeSpriteRender += new System.EventHandler<System.Windows.Forms.PaintEventArgs>(this.surface_BeforeSpriteRender);
            this.surface.RangeSelected += new System.EventHandler<SCG.TurboSprite.RangeSelectedEventArgs>(this.surface_RangeSelected);
            this.surface.SpriteCollision += new System.EventHandler<SCG.TurboSprite.SpriteCollisionEventArgs>(this.surface_SpriteCollision);
            this.surface.MouseDown += new System.Windows.Forms.MouseEventHandler(this.surface_MouseDown);
            this.surface.SpriteClicked += new System.EventHandler<SCG.TurboSprite.SpriteClickEventArgs>(this.surface_SpriteClicked);
            // 
            // engineDest
            // 
            this.engineDest.DetectCollisionSelf = true;
            this.engineDest.DetectCollisionTag = 0;
            this.engineDest.Priority = 1;
            this.engineDest.Surface = this.surface;
            this.engineDest.SpriteReachedDestination += new System.EventHandler<SCG.TurboSprite.SpriteEventArgs>(this.engineDest_SpriteReachedDestination);
            // 
            // engineStars
            // 
            this.engineStars.DetectCollisionSelf = false;
            this.engineStars.DetectCollisionTag = 1;
            this.engineStars.Priority = 2;
            this.engineStars.Surface = this.surface;
            // 
            // pieces
            // 
            this.pieces.CellsX = 48;
            this.pieces.CellsY = 1;
            this.pieces.MasterBitmap = ((System.Drawing.Bitmap)(resources.GetObject("pieces.MasterBitmap")));
            // 
            // TurboSpriteTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 434);
            this.Controls.Add(this.btnShockWave);
            this.Controls.Add(this.btnParticle);
            this.Controls.Add(this.pnlColor);
            this.Controls.Add(this.btnPiece);
            this.Controls.Add(this.btnPolygon);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.picGlyph);
            this.Controls.Add(this.lblSprites);
            this.Controls.Add(this.lblFPS);
            this.Controls.Add(this.btnAddSprite);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.surface);
            this.Name = "TurboSpriteTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TurboSprite Demo";
            ((System.ComponentModel.ISupportInitialize)(this.picGlyph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SCG.TurboSprite.SquareGridSpriteSurface surface;
        private System.Windows.Forms.Button btnActivate;
        private SCG.TurboSprite.SpriteEngineDestination engineDest;
        private System.Windows.Forms.Button btnAddSprite;
        private System.Windows.Forms.Label lblFPS;
        private System.Windows.Forms.Label lblSprites;
        private System.Windows.Forms.PictureBox picGlyph;
        private System.Windows.Forms.Button button1;
        private SCG.TurboSprite.SpriteEngine engineStars;
        private System.Windows.Forms.Button btnPolygon;
        private SCG.TurboSprite.GamePieceBitmapFactory pieces;
        private System.Windows.Forms.Button btnPiece;
        private System.Windows.Forms.Panel pnlColor;
        private System.Windows.Forms.ColorDialog dlgColor;
        private System.Windows.Forms.Button btnParticle;
        private System.Windows.Forms.Button btnShockWave;
    }
}

