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
    public class StarFieldSprite : Sprite
    {
        private int _numStars;
        private int _speed;
        private Star[] _starArray;
        private int _q1;
        private int _q2;
        private int _q3;

        public StarFieldSprite(int numStars, int width, int height, int speed)
        {
            Shape = new RectangleF(width / 2, height / 2, width, height);
            _numStars = numStars;
            _speed = speed;
            _starArray = new Star[numStars];
            for(int i = 0; i < numStars; i++)         
            {
                Star s = new Star();
                _starArray[i] = s;
                do
                {
                    s.X = _rnd.Next(Width) - WidthHalf;
                    s.Y = _rnd.Next(Height) - HeightHalf;
                }
                while (s.X == 0 || s.Y == 0);
                s.Z = i;
            }
            _q1 = numStars / 4 * 3;
            _q2 = numStars / 2;
            _q3 = numStars / 4;

            addProcessHandler(sprite => {
                for (int i = 0; i < _numStars; i++)
                {
                    Star s = _starArray[i];
                    s.Z = s.Z - _speed;
                    if (s.Z < 0)
                        s.Z += _numStars;
                    else if (s.Z >= _numStars)
                        s.Z -= _numStars;
                }

            });
        }

        // Internal struct used to represent a single star
        class Star
        {
            internal int X;
            internal int Y;
            internal int Z;
        }

        // Four quadrant colors
        private static Color _color1 = Color.FromArgb(255, 255, 255);
        private static Color _color2 = Color.FromArgb(204, 204, 204);
        private static Color _color3 = Color.FromArgb(163, 163, 163);
        private static Color _color4 = Color.FromArgb(131, 131, 131);

        // Random Number Generator
        private static Random _rnd = new Random(DateTime.Now.Millisecond);

        // Render
        protected internal override void Render(Graphics g)
        {
            int x;
            int y;
            Pen p;
            using (Pen p1 = new Pen(_color1), p2 = new Pen(_color2), p3 = new Pen(_color3), p4 = new Pen(_color4))
            {
                for (int i = 0; i < _numStars; i++)
                {
                    Star s = _starArray[i];
                    if (s.Z != 0)
                    {
                        x = s.X * 256 / s.Z;
                        y = s.Y * 256 / s.Z;
                        if (s.Z >= _q1)
                            p = p4;
                        else if (s.Z >= _q2)
                            p = p3;
                        else if (s.Z >= _q1)
                            p = p2;
                        else
                            p = p1;
                        g.DrawRectangle(p, x + X - Surface.OffsetX, y + Y - Surface.OffsetY, 1, 1);
                    }
                }
            }
        }

    }
}
