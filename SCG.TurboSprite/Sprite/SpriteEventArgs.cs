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
using System.Windows.Forms;

namespace SCG.TurboSprite
{
    // EventArgs that contains Sprite parameter
    public class SpriteEventArgs : EventArgs
    {
        public SpriteEventArgs(Sprite sprite)
        {
            Sprite = sprite;
        }

        public Sprite Sprite { get; }
    }

    public class SpriteMoveEventArgs : SpriteEventArgs
    {
        public SpriteMoveEventArgs(Sprite sprite) : base(sprite)
        {
        }

        public float Distance
        {
            get
            {
                TargetMover mover = (TargetMover)Sprite.Mover;
                return (float)Sprite.GetDistance(mover.LastPositionX, Sprite.X, mover.LastPositionY, Sprite.Y);
            }
        }

        public float Speed
        {
            get
            {
                TargetMover mover = (TargetMover)Sprite.Mover;
                return (float)Math.Sqrt((mover.SpeedX * mover.SpeedX) + (mover.SpeedY * mover.SpeedY));
            }
        }
    }

    public class SpriteClickEventArgs : SpriteEventArgs
    {
        public SpriteClickEventArgs(Sprite sprite, MouseButtons button) : base(sprite)
        {
            Button = button;
        }

        public MouseButtons Button { get; }
    }

    // Collision detection event
    public class SpriteCollisionEventArgs : EventArgs
    {
        public SpriteCollisionEventArgs(Sprite sprite1, Sprite sprite2)
        {
            Sprite1 = sprite1;
            Sprite2 = sprite2;
        }

        public Sprite Sprite1 { get; }

        public Sprite Sprite2 { get; }
    }
}
