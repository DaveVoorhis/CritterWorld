#region copyright
/*
* Copyright (c) 2008, Dion Kurczek
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
using System.Text;
using System.Drawing;

namespace SCG.TurboSprite
{
    public class ShockWaveSprite : Sprite
    {
        private List<Wave> _waves = new List<Wave>();
        private int _lifeSpan;

        public ShockWaveSprite(int waves, float radius, int lifeSpan, Color startColor, Color endColor)
        {
            _lifeSpan = lifeSpan;
            while (waves > 0)
            {
                Wave wave = new Wave();
                wave.Color = Sprite.ColorFromRange(startColor, endColor);
                wave.Radius = radius;
                _waves.Add(wave);
                waves--;
            }
            Shape = new RectangleF(-50, -50, 100, 100);
            addProcessHandler(sprite =>
            {
                // decrease life span
                _lifeSpan--;
                if (_lifeSpan <= 0)
                    Kill();
            });
        }

        // Render the sprite
        protected internal override void Render(System.Drawing.Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            foreach (Wave wave in _waves)
            {
                wave.Radius += wave.ExpansionRate;
                float w = wave.Radius * 2 + Sprite.RND.Next(6) - 3;
                float h = wave.Radius * 2 + Sprite.RND.Next(6) - 3;
                float x = X - wave.Radius + Sprite.RND.Next(6) - 3;
                float y = Y - wave.Radius + Sprite.RND.Next(6) - 3;
                pen.Color = wave.Color;
                g.DrawEllipse(pen, x - Surface.OffsetX, y - Surface.OffsetY, w, h);
            }
        }
    }

    internal class Wave
    {
        internal float ExpansionRate = (float)(Sprite.RND.NextDouble() * Sprite.RND.NextDouble() * Sprite.RND.NextDouble() * 6);
        internal Color Color;
        internal float Radius;
    }
}
