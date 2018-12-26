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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCG.TurboSprite
{
    // Contains the information necessary to process the movement of a sprite towards
    // its destination.
    public class DestinationMover
    {
        private Sprite _sprite;
        private float _speed;
        private float _destX;
        private float _destY;
        private float _speedX;
        private float _speedY;
        private bool _stopAtDestination = true;

        // Constructor
        public DestinationMover(Sprite sprite)
        {
            _sprite = sprite;
        }

        // Sprite's speed
        public float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        public float SpeedX
        {
            get
            {
                return _speedX;
            }
            set
            {
                _speedX = value;
            }
        }

        public float SpeedY
        {
            get
            {
                return _speedY;
            }
            set
            {
                _speedY = value;
            }
        }

        // Sprite's destination
        public float DestX
        {
            get
            {
                return _destX;
            }
            set
            {
                _destX = value;
                CalculateVectors();
            }
        }

        public float DestY
        {
            get
            {
                return _destY;
            }
            set
            {
                _destY = value;
                CalculateVectors();
            }
        }

        public PointF DestinationF
        {
            get
            {
                return new PointF(_destX, _destY);
            }
            set
            {
                _destX = value.X;
                _destY = value.Y;
                CalculateVectors();
            }
        }

        public Point Destination
        {
            get
            {
                return new Point((int)_destX, (int)_destY);
            }
            set
            {
                _destX = value.X;
                _destY = value.Y;
                CalculateVectors();
            }
        }

        // Should the sprite stop moving once it reaches its destination?
        public bool StopAtDestination
        {
            get
            {
                return _stopAtDestination;
            }
            set
            {
                _stopAtDestination = value;
            }
        }

        // Calculate X/Y movement vectors based on speed and destination
        private void CalculateVectors()
        {
            float Dist = Math.Abs(DestinationF.X - _sprite.PositionF.X) + Math.Abs(DestinationF.Y - _sprite.PositionF.Y);
            if (Dist > 0)
            {
                float PctX = Math.Abs(DestinationF.X - _sprite.PositionF.X) / Dist;
                float PctY = Math.Abs(DestinationF.Y - _sprite.PositionF.Y) / Dist;
                _speedX = _speed * PctX;
                _speedY = _speed * PctY;
                if (DestinationF.X < _sprite.PositionF.X)
                    _speedX = -_speedX;
                if (DestinationF.Y < _sprite.PositionF.Y)
                    _speedY = -_speedY;
            }
            else
            {
                _speedX = _speed / 2;
                _speedY = _speedX;
            }
        }

        // Move the sprite, called by SpriteEngine's MoveSprite method
        internal void MoveSprite()
        {
            if (_speedX == 0 && _speedY == 0)
                return;
            // Do not check destination, just move the sprite
            if (!_stopAtDestination)
            {
                _sprite.X += _speedX;
                _sprite.Y += _speedY;
            }
            // Check to see if it stopped at its destination
            else
            {
                float Temp;
                // Check X-Axis movement
                Temp = _sprite.PositionF.X + _speedX;
                if (_speedX > 0 && Temp > DestinationF.X)
                    _sprite.X = DestX;
                else if (_speedX < 0 && Temp < DestinationF.X)
                    _sprite.X = DestX;
                else
                    _sprite.X += SpeedX;
                // Check Y-Axis movement
                Temp = _sprite.PositionF.Y + _speedY;
                if (_speedY > 0 && Temp > DestinationF.Y)
                    _sprite.Y = DestY;
                else if (_speedY < 0 && Temp < DestinationF.Y)
                    _sprite.Y = DestY;
                else
                    _sprite.Y += SpeedY;
            }
            _sprite.NotifyMoved();
        }
    }
}
