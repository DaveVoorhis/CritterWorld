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
using System.Drawing.Drawing2D;

namespace SCG.TurboSprite
{
    public class TextSprite : Sprite
    {
        private GraphicsPath path;
        private Point oldPosition = new Point(-1, -1);
        private string oldText = null;
        private string oldFontName = null;
        private int oldSize = -1;
        private FontStyle oldStyle = FontStyle.Regular;

        public string Text { get; set; }

        public string FontName { get; set; }

        public int Size { get; set; }

        public FontStyle Style { get; set; }

        // Construct a vector sprite from text.
        public TextSprite(string text, string fontName, int size, FontStyle style)
        {
            Text = text;
            FontName = fontName;
            Size = size;
            Style = style;
            Shape = new Rectangle(1, 1, 1, 1);
        }

        private void ObtainShape()
        {
            // Set the shape of the sprite based on largest dimension from center
            float x1 = 0;
            float y1 = 0;
            float x2 = 0;
            float y2 = 0;
            foreach (PointF point in path.PathPoints)
            {
                if (point.X < x1)
                {
                    x1 = point.X;
                }
                if (point.X > x2)
                {
                    x2 = point.X;
                }
                if (point.Y < y1)
                {
                    y1 = point.Y;
                }
                if (point.Y > y2)
                {
                    y2 = point.Y;
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

        // Transparency. 255 = opaque. 0 = transparent.
        public byte Alpha { get; set; } = 255;

        // String alignment
        public StringAlignment HorizontalAlignment { get; set; } = StringAlignment.Center;

        // String alignment
        public StringAlignment VerticalAlignment { get; set; } = StringAlignment.Center;

        // Render the sprite - draw the polygon
        protected internal override void Render(Graphics graphics)
        {
            if (Text != oldText || !Position.Equals(oldPosition) || FontName != oldFontName || Size != oldSize || Style != oldStyle)
            {
                path = new GraphicsPath();

                using (Font font = new Font(FontName, Size, Style))
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.Alignment = HorizontalAlignment;
                    stringFormat.LineAlignment = VerticalAlignment;
                    path.AddString(Text, font.FontFamily, (int)font.Style, font.Size, Position, stringFormat);
                }
                ObtainShape();

                oldText = Text;
                oldPosition = Position;
                oldFontName = FontName;
                oldSize = Size;
                oldStyle = Style;
            }

            // Fill it?
            if (IsFilled)
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(Alpha, Color)))
                {
                    graphics.FillPath(brush, path);
                }
            }

            // Draw outline
            using (Pen pen = new Pen(Color.FromArgb(Alpha, Color), LineWidth))
            {
                graphics.DrawPath(pen, path);
            }
        }
    }
}
