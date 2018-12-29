#region copyright
/*
* Copyright (c) 2018, Dave Voorhis
* Based on PolygonSprite by Dion Kurczek
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
* THIS SOFTWARE IS PROVIDED BY DAVE VOORHIS ``AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL DAVE VOORHIS BE LIABLE FOR ANY
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
    public class PolygonSprite : Sprite
    {
        private int _lastFrame = 0;
        private int _lastAngle;
        private readonly PointF[][] _rotatedPoints;
        private readonly PointF[][] _drawnPoints;
        private readonly PointF[][] _model;

        // Construct a polygon-based sprite with 1 or more arrays of points that can be selected to create animation.
        public PolygonSprite(PointF[][] model)
        {
            _model = model;
            _lastAngle = -1;
            _lastFrame = 0;
            FrameCount = model.Length;
            _rotatedPoints = new PointF[FrameCount][];
            _drawnPoints = new PointF[FrameCount][];
            for (int i = 0; i < FrameCount; i++)
            {
                int polySize = model[i].Length;
                _drawnPoints[i] = new PointF[polySize];
                _rotatedPoints[i] = new PointF[polySize];
            }
            RotateAndAnimate();
        }

        // Construct a non-animated polygon-based sprite.
        public PolygonSprite(PointF[] points) : this(new PointF[][] { points }) {}

        // Select the specific Points collection to display.
        public int Frame { get; set; }

        // Get the number of animation frames.
        public int FrameCount { get; internal set; }

        // Switch to next frame.
        public void IncrementFrame()
        {
            int nextFrame = Frame + 1;
            if (nextFrame >= FrameCount)
            {
                nextFrame = 0;
            }
            Frame = nextFrame;
        }

        private void ObtainShape()
        {
            // Set the shape of the sprite based on largest dimension from center
            float x1 = 0;
            float y1 = 0;
            float x2 = 0;
            float y2 = 0;
            foreach (PointF pt in _rotatedPoints[_lastFrame])
            {
                if (pt.X < x1)
                {
                    x1 = pt.X;
                }
                if (pt.X > x2)
                {
                    x2 = pt.X;
                }
                if (pt.Y < y1)
                {
                    y1 = pt.Y;
                }
                if (pt.Y > y2)
                {
                    y2 = pt.Y;
                }
            }
            Shape = new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        // Access line color
        public Color Color { get; set; } = Color.Red;

        // Access line width
        public int LineWidth { get; set; }

        // Determine whether the Sprite is filled
        public bool IsFilled { get; set; }

        // Access the fill color
        public Color FillColor { get; set; } = Color.Empty;

        private void RotateAndAnimate()
        {
            // Process rotation and animation
            if (FacingAngle != _lastAngle || Frame != _lastFrame)
            {
                _lastAngle = FacingAngle;
                _lastFrame = Frame;
                float sin = Sprite.Sin(FacingAngle);
                float cos = Sprite.Cos(FacingAngle);
                for (int p = 0; p < _rotatedPoints[_lastFrame].Length; p++)
                {
                    _rotatedPoints[_lastFrame][p].X = _model[_lastFrame][p].X * cos - _model[_lastFrame][p].Y * sin;
                    _rotatedPoints[_lastFrame][p].Y = _model[_lastFrame][p].Y * cos + _model[_lastFrame][p].X * sin;
                }
                ObtainShape();
            }
        }

        // Render the sprite - draw the polygon
        protected internal override void Render(System.Drawing.Graphics g)
        {
            RotateAndAnimate();

            // Transform polygon into viewport coordinates
            for (int pt = 0; pt < _rotatedPoints[_lastFrame].Length; pt++)
            {
                _drawnPoints[_lastFrame][pt].X = _rotatedPoints[_lastFrame][pt].X + X - Surface.OffsetX;
                _drawnPoints[_lastFrame][pt].Y = _rotatedPoints[_lastFrame][pt].Y + Y - Surface.OffsetY;
            }

            // Make pretty
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Fill it?
            if (IsFilled)
            {
                Brush brush = new SolidBrush(FillColor);
                using (brush)
                {
                    g.FillPolygon(brush, _drawnPoints[_lastFrame]);
                }
            }

            // Draw outline
            Pen pen = new Pen(Color, LineWidth);
            using(pen)
            {
                g.DrawPolygon(pen, _drawnPoints[_lastFrame]);
            }
        }
    }
}
