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
        private int _frame = 0;
        private PointF[][] _points;
        private PointF[][] _drawnPoints;
        private PointF[][] _unrotated;
        private Color _color = Color.Red;
        private int _width;
        private bool _filled;
        private Color _fillColor = Color.Empty;
        private int _lastAngle;
        private int _nextFrame;
        private int _frameCount;

        // Construct a polygon-based sprite with 1 or more arrays of points that can be selected.
        public PolygonSprite(PointF[][] points)
        {
            _frame = 0;
            _frameCount = points.Length;
            _points = new PointF[_frameCount][];
            _drawnPoints = new PointF[_frameCount][];
            _unrotated = new PointF[_frameCount][];
            for (int i = 0; i < _frameCount; i++)
            {
                int polySize = points[i].Length;
                _drawnPoints[i] = new PointF[polySize];
                _unrotated[i] = new PointF[polySize];
                _points[i] = new PointF[polySize];
                points[i].CopyTo(_unrotated[i], 0);
                points[i].CopyTo(_points[i], 0);
            }
            obtainShape();
        }

        // Construct a non-animated polygon-based sprite.
        public PolygonSprite(PointF[] points) : this(new PointF[][] { points }) {}

        // Select the specific Points collection to display.
        public int NextFrame
        {
            get
            {
                return _nextFrame;
            }
            set
            {
                _nextFrame = value;
            }
        }

        override public void NotifyMoved()
        {
            int nextFrame = NextFrame + 1;
            if (nextFrame >= _frameCount)
                nextFrame = 0;
            NextFrame = nextFrame;
        }

        private void obtainShape()
        {
            // Set the shape of the sprite based on largest dimension from center
            float x1 = 0;
            float y1 = 0;
            float x2 = 0;
            float y2 = 0;
            foreach (PointF pt in _points[_frame])
            {
                if (pt.X < x1)
                    x1 = pt.X;
                if (pt.X > x2)
                    x2 = pt.X;
                if (pt.Y < y1)
                    y1 = pt.Y;
                if (pt.Y > y2)
                    y2 = pt.Y;
            }
            Shape = new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        // Access line color
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        // Access line width
        public int LineWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        // Determine whether the Sprite is filled
        public bool IsFilled
        {
            get
            {
                return _filled;
            }
            set
            {
                _filled = value;
            }
        }

        // Access the fill color
        public Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
            }
        }

        // Render the sprite - draw the polygon
        protected internal override void Render(System.Drawing.Graphics g)
        {
            // Process rotation and animation
            if (FacingAngle != _lastAngle || _nextFrame != _frame)
            {
                _lastAngle = FacingAngle;
                _frame = _nextFrame;
                float sin = Sprite.Sin(FacingAngle);
                float cos = Sprite.Cos(FacingAngle);
                for (int p = 0; p < _points[_frame].Length; p++)
                {
                    _points[_frame][p].X = _unrotated[_frame][p].X * cos - _unrotated[_frame][p].Y * sin;
                    _points[_frame][p].Y = _unrotated[_frame][p].Y * cos + _unrotated[_frame][p].X * sin;
                }
                obtainShape();
            }

            // Transform polygon into viewport coordinates
            for (int pt = 0; pt < _points[_frame].Length; pt++)
            {
                _drawnPoints[_frame][pt].X = _points[_frame][pt].X + X - Surface.OffsetX;
                _drawnPoints[_frame][pt].Y = _points[_frame][pt].Y + Y - Surface.OffsetY;
            }

            // Make pretty
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Fill it?
            if (_filled)
            {
                Brush brush = new SolidBrush(_fillColor);
                using (brush)
                {
                    g.FillPolygon(brush, _drawnPoints[_frame]);
                }
            }

            // Draw outline
            Pen pen = new Pen(_color, _width);
            using(pen)
            {
                g.DrawPolygon(pen, _drawnPoints[_frame]);
            }
        }
    }
}
