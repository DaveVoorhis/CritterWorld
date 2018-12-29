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
    public class AnimatedBitmapSprite : Sprite
    {
        private GamePieceBitmapFactory _gpbf;
        private int _row;
        private int _counter = 0;
        private int _widthHalf;
        private int _heightHalf;

        // Constructor - pass a GPSF, and desired row to use
        public AnimatedBitmapSprite(GamePieceBitmapFactory gpbf, int row)
        {
            _gpbf = gpbf;
            _widthHalf = gpbf.CellWidth / 2;
            _heightHalf = gpbf.CellHeight / 2;
            _row = row;
            Shape = new System.Drawing.RectangleF(-gpbf.CellWidth / 2, -gpbf.CellHeight / 2, gpbf.CellWidth, gpbf.CellHeight);
        }

        // The row to use
        public int Row
        {
            get
            {
                return _row;
            }
            set
            {
                if (value < _gpbf.CellsY)
                {
                    _row = value;
                }
            }
        }

        // The latency, number of cycles to wait before advancing frame
        public int FrameLatency { get; set; } = 10;

        // Allow access to the frame
        public int Frame { get; set; } = 0;

        // Render the sprite
        protected internal override void Render(Graphics g)
        {
            // advance the counter/frame
            _counter++;
            if (_counter >= FrameLatency)
            {
                _counter = 0;
                Frame++;
                if (Frame >= _gpbf.CellsX)
                {
                    Frame = 0;
                }
            }

            // get the appropriate cell
            Bitmap bmp = _gpbf.GetGamePieceBitmap(Frame, _row);

            // draw it
            g.DrawImage(bmp, X - _widthHalf - Surface.OffsetX, Y - _heightHalf - Surface.OffsetY);
        }
    }
}
