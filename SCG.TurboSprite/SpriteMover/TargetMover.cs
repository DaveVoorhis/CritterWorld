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
    // Move a Sprite toward a target.
    public class TargetMover : IMover
    {
        private Sprite _sprite;
        private float _targetX;
        private float _targetY;
        private int _targetFacingAngle;

        public event EventHandler<SpriteEventArgs> SpriteReachedTarget;
        public event EventHandler<SpriteEventArgs> SpriteMoved;

        // Sprite's speed
        public float Speed { get; set; }

        public float SpeedX { get; set; }

        public float SpeedY { get; set; }

        // Last position before movement.
        public float OldX { get; set; }
        public float OldY { get; set; }

        public PointF LastPosition
        {
            get
            {
                return new PointF(OldX, OldY);
            }
        }

        // Desired orientation. Sprite's FacingAngle will be smoothly updated on each movement to eventually face this way.
        public int TargetFacingAngle
        {
            get
            {
                return _targetFacingAngle;
            }
            set
            {
                _targetFacingAngle = Sprite.NormaliseAngle(value);
            }
        }

        // Sprite's destination
        public float TargetX
        {
            get
            {
                return _targetX;
            }
            set
            {
                _targetX = value;
                CalculateVectors();
            }
        }

        public float TargetY
        {
            get
            {
                return _targetY;
            }
            set
            {
                _targetY = value;
                CalculateVectors();
            }
        }

        public Point Target
        {
            get
            {
                return new Point((int)_targetX, (int)_targetY);
            }
            set
            {
                _targetX = value.X;
                _targetY = value.Y;
                CalculateVectors();
            }
        }

        // Should the sprite stop moving once it reaches its destination?
        public bool StopAtTarget { get; set; } = true;

        // Calculate X/Y movement vectors based on speed and destination
        private void CalculateVectors()
        {
            if (_sprite == null)
            {
                return;
            }
            float distance = Math.Abs(TargetX - _sprite.X) + Math.Abs(TargetY - _sprite.Y);
            if (distance > 0)
            {
                float PctX = Math.Abs(TargetX - _sprite.X) / distance;
                float PctY = Math.Abs(TargetY - _sprite.Y) / distance;
                SpeedX = Speed * PctX;
                SpeedY = Speed * PctY;
                if (TargetX < _sprite.X)
                {
                    SpeedX = -SpeedX;
                }
                if (TargetY < _sprite.Y)
                {
                    SpeedY = -SpeedY;
                }
            }
            else
            {
                SpeedX = Speed / 2;
                SpeedY = SpeedX;
            }
        }

        // Move the sprite, called by SpriteEngine's MoveSprite method
        public void MoveSprite(Sprite sprite)
        {
            if (_sprite == null)
            {
                _sprite = sprite;
                CalculateVectors();
            }
            if (_sprite.FacingAngle != TargetFacingAngle)
            {
                int distance = Math.Abs(_sprite.FacingAngle - TargetFacingAngle);
                int step = distance / 4;
                int turn = (_sprite.FacingAngle < TargetFacingAngle) ? step : -step;
                _sprite.FacingAngle += (distance < 180) ? turn : -turn;
            }
            if (SpeedX == 0 && SpeedY == 0)
            {
                return;
            }
            OldX = _sprite.X;
            OldY = _sprite.Y;
            if (!StopAtTarget)
            {
                // Do not check destination, just move the sprite
                _sprite.X += SpeedX;
                _sprite.Y += SpeedY;
            }
            else
            {
                // Handle reaching destination
                float TempX = _sprite.X + SpeedX;
                _sprite.X = ((SpeedX > 0 && TempX > TargetX) || (SpeedX < 0 && TempX < TargetX)) ? TargetX : _sprite.X + SpeedX;
                float TempY = _sprite.Y + SpeedY;
                _sprite.Y = ((SpeedY > 0 && TempY > TargetY) || (SpeedY < 0 && TempY < TargetY)) ? TargetY : _sprite.Y + SpeedY;
            }
            // If sprite moved, alert listeners
            if (SpriteMoved != null && (_sprite.X != OldX || _sprite.Y != OldY))
            {
                SpriteMoved(this, new SpriteEventArgs(_sprite));
            }
            // If sprite has reached its target, alert listeners
            if (SpriteReachedTarget != null && sprite.Position == Target)
            {
                SpriteReachedTarget(this, new SpriteEventArgs(_sprite));
            }
        }
    }
}
