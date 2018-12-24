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
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;

namespace SCG.TurboSprite
{
    // SpriteMoverDestination - implements logic to move sprites toward a specific
    // X, Y destination, at a specific speed.
    public partial class SpriteEngineDestination : SpriteEngine
    {
        // Constructors
        public SpriteEngineDestination()
        {
            InitializeComponent();
        }

        public SpriteEngineDestination(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        // events
        public event EventHandler<SpriteEventArgs> SpriteReachedDestination;

        //  Access a sprite's DestinationMover object
        public DestinationMover GetMover(Sprite sprite)
        {
            DestinationMover dm = (DestinationMover)sprite.MovementData;
            return dm;
        }

        // Create a DestinationMove object and attach it to the sprite
        protected override void InitializeSprite(SCG.TurboSprite.Sprite sprite)
        {
            sprite.MovementData = new DestinationMover(sprite);
        }

        // Process movement of a sprite toward its destination
        protected override void MoveSprite(SCG.TurboSprite.Sprite sprite)
        {
            // Allow the mover object to perform the actual movement
            DestinationMover sd = (DestinationMover)sprite.MovementData;
            sd.MoveSprite();
            // If sprite has reached its target destination, alert the client app
            if (SpriteReachedDestination != null)
                if (sprite.Position == sd.Destination)
                    SpriteReachedDestination(this, new SpriteEventArgs(sprite));
        }
    }
}
