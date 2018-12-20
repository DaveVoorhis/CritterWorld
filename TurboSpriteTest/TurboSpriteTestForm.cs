#region copyright
/*
* Copyright (c) 2007, Dion Kurczek
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of the <organization> nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY DION KURCZEK ``AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL DION KURCZEK BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SCG.TurboSprite;

namespace TurboSpriteTest
{
    public partial class TurboSpriteTestForm : Form
    {
        private Random rnd = new Random(DateTime.Now.Millisecond);

        public TurboSpriteTestForm()
        {
            InitializeComponent();
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            surface.Active = !surface.Active;
        }

        private void surface_BeforeSpriteRender(object sender, PaintEventArgs e)
        {
            lblFPS.Text = "FPS: " + surface.ActualFPS.ToString();
            lblSprites.Text = "Sprites: " + engineDest.Sprites.Count.ToString();
        }

        private void btnAddSprite_Click(object sender, EventArgs e)
        {
            //Create a BitmapSprite
            BitmapSprite s = new BitmapSprite((Bitmap)picGlyph.Image);
            s.Bitmap.MakeTransparent(Color.Black);

            //Center it on the SpriteSurface
            s.Position = new Point(surface.Width / 2, surface.Height / 2);

            //Add it to the SpriteEngine
            engineDest.AddSprite(s);

            //Set its speed and destination
            DestinationMover dm = engineDest.GetMover(s);
            dm.Speed = rnd.Next(10) + 1;
            dm.Destination = new Point(rnd.Next(surface.Width), rnd.Next(surface.Height));
            dm.StopAtDestination = false;          
        }

        private void engineDest_SpriteReachedDestination(object sender, SpriteEventArgs e)
        {           
        }

        private void surface_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (Sprite s in engineDest.Sprites)
            {
                DestinationMover dm = engineDest.GetMover(s);
                if (e.Button == MouseButtons.Right)
                    dm.Speed = 20;
                dm.Destination = new Point(e.X, e.Y);             
            }
        }

        private void surface_SpriteCollision(object sender, SpriteCollisionEventArgs e)
        {
            DestinationMover dm1 = engineDest.GetMover(e.Sprite1);
            DestinationMover dm2 = engineDest.GetMover(e.Sprite2);
            float sx1 = dm1.SpeedX;
            float sy1 = dm1.SpeedY;
            float sx2 = dm2.SpeedX;
            float sy2 = dm2.SpeedY;
            dm1.SpeedX = sx2;
            dm1.SpeedY = sy2;
            dm2.SpeedX = sx1;
            dm2.SpeedY = sy1;
            e.Sprite1.Kill();
            e.Sprite2.Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StarFieldSprite sfs = new StarFieldSprite(100, 50, 50, 1);
            sfs.Position = new Point(surface.Width / 2, surface.Height / 2);
            engineStars.AddSprite(sfs);
        }

        private void scrollHorz_Scroll(object sender, ScrollEventArgs e)
        {
            surface.OffsetX = e.NewValue;
        }

        private void scrollVert_Scroll(object sender, ScrollEventArgs e)
        {
            surface.OffsetY = e.NewValue;
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            PointF[] poly = { new PointF(0, -10), new PointF(5, 10), new PointF(0, 5), new PointF(-5, 10) };
            PolygonSprite s = new PolygonSprite(poly);
            if (rnd.Next(100) > 50)
            {
                if (rnd.Next(100) > 50)
                    s.Spin = SpinType.Clockwise;
                else
                    s.Spin = SpinType.CounterClockwise;
                s.SpinSpeed = rnd.Next(20) + 1;
            }
            s.Position = new Point(surface.Width / 2, surface.Height / 2);
            engineDest.AddSprite(s);
            DestinationMover dm = engineDest.GetMover(s);
            dm.Speed = rnd.Next(10) + 1;
            dm.Destination = new Point(rnd.Next(surface.Width), rnd.Next(surface.Height));
            dm.StopAtDestination = false;          
        }

        private void btnPiece_Click(object sender, EventArgs e)
        {
            Bitmap piece = pieces.GetGamePieceBitmap(rnd.Next(48), 0, pnlColor.BackColor);
            BitmapSprite s = new BitmapSprite(piece);
            s.Bitmap.MakeTransparent(Color.Black);
            s.Position = new Point(surface.Width / 2, surface.Height / 2);
            engineDest.AddSprite(s);
            DestinationMover dm = engineDest.GetMover(s);
            dm.Speed = rnd.Next(10) + 1;
            dm.Destination = new Point(rnd.Next(surface.Width), rnd.Next(surface.Height));
            dm.StopAtDestination = false;    
        }

        private void pnlColor_Click(object sender, EventArgs e)
        {
            dlgColor.Color = pnlColor.BackColor;
            if (dlgColor.ShowDialog() == DialogResult.OK)
                pnlColor.BackColor = dlgColor.Color;
        }

        private void surface_RangeSelected(object sender, RangeSelectedEventArgs e)
        {
            Text = "Range Selected: " + e.SelectedRange.X + "," + e.SelectedRange.Y + " " + e.SelectedRange.Width + "," + e.SelectedRange.Height;
        }

        private void surface_SpriteClicked(object sender, SpriteEventArgs e)
        {
            Text = e.Sprite.GetType().Name;
        }

        private void btnParticle_Click(object sender, EventArgs e)
        {
            ParticleExplosionSprite pes = new ParticleExplosionSprite(30, Color.Yellow, Color.Red, 2, 4, 30);
            engineDest.AddSprite(pes);
            pes.Position = new Point(surface.Width / 2, surface.Height / 2);
        }

        private void btnShockWave_Click(object sender, EventArgs e)
        {
            ShockWaveSprite sw = new ShockWaveSprite(50, 20, 15, Color.Black, Color.Lime);
            engineDest.AddSprite(sw);
            sw.Position = new Point(surface.Width / 2, surface.Height / 2);
        }

        private void surface_SpriteClicked(object sender, SpriteClickEventArgs e)
        {

        }

    }
}