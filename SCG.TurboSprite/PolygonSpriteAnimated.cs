#region copyright
/*
* Copyright (c) 2008, Dion Kurczek
* Modifications copyright (c) 2018, Dave Voorhis
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
    public class PolygonSpriteAnimated : Sprite
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
        private int _lastFrame;

        //Construct a PolygonSprite with 1 or more arrays of points that can be selected.
        public PolygonSpriteAnimated(params PointF[][] points)
        {
            _frame = 0;
            _points = new PointF[points.Length][];
            _drawnPoints = new PointF[points.Length][];
            _unrotated = new PointF[points.Length][];
            Points = points[_frame];
            for (int i = 0; i < points.Length; i++)
            {
                _drawnPoints[i] = new PointF[points[i].Length];
                _unrotated[i] = new PointF[points[i].Length];
                points[i].CopyTo(_unrotated[i], 0);
            }
        }

        // Select the specific Points collection to display.
        public int Frame
        {
            get
            {
                return _frame;
            }
            set
            {
                _frame = value;
            }
        }

        //Access Points collection, allow it to change
        public PointF[] Points
        {
            get
            {
                return _points[_frame];
            }
            set
            {
                _points[_frame] = value;

                //Set the shape of the sprite based on largest dimension from center
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
        }

        //Access line color
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

        //Access line width
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

        //Determine whether the Sprite is filled
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

        //Access the fill color
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

        //Process the sprite on each animation cycle - handle rotation
        protected internal override void Process()
        {
            //Process rotation of shape
            if (FacingAngle != _lastAngle || Frame != _lastFrame)
            {
                float sin = Sprite.Sin(FacingAngle);
                float cos = Sprite.Cos(FacingAngle);
                _lastAngle = FacingAngle;
                _lastFrame = Frame;
                for (int p = 0; p < _points.Length; p++)
                {
                    _points[_frame][p].X = _unrotated[_frame][p].X * cos - _unrotated[_frame][p].Y * sin;
                    _points[_frame][p].Y = _unrotated[_frame][p].Y * cos + _unrotated[_frame][p].X * sin;
                }

                //This causes shape to be correctly recalculated
                Points = _points[_frame];
            }
        }

        //Render the sprite - draw the polygon
        protected internal override void Render(System.Drawing.Graphics g)
        {
            //Transform polygon into viewport coordinates
            for(int pt = 0; pt < _points.Length; pt++)
            {
                _drawnPoints[_frame][pt].X = _points[_frame][pt].X + X - Surface.OffsetX;
                _drawnPoints[_frame][pt].Y = _points[_frame][pt].Y + Y - Surface.OffsetY;
            }

            //Fill it?
            if (_filled)
            {
                Brush brush = new SolidBrush(_fillColor);
                using (brush)
                {
                    g.FillPolygon(brush, _drawnPoints[_frame]);
                }
            }

            //Draw outline
            Pen pen = new Pen(_color, _width);
            using(pen)
            {
                g.DrawPolygon(pen, _drawnPoints[_frame]);
            }
        }
    }
}
