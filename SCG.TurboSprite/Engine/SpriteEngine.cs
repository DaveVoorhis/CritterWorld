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
    // Manages a collection of sprites
    public partial class SpriteEngine : Component, IComparable
    {
        private SpriteSurface _surface;
        private int _priority = 1;
        internal List<Sprite> _spriteList = new List<Sprite>();

        public event EventHandler<SpriteEventArgs> SpriteRemoved;

        public SpriteEngine()
        {
            InitializeComponent();
        }

        public SpriteEngine(IContainer container)
        {
            container.Add(this);
            InitializeComponent();            
        }

        // The SpriteSurface this SpriteEngine is associated with
        public SpriteSurface Surface
        {
            get
            {
                return _surface;
            }
            set
            {
                if (_surface != null)
                {
                    _surface.UnRegisterSpriteEngine(this);
                }
                if (value != null)
                {
                    value.RegisterSpriteEngine(this);
                }
                _surface = value;
            }
        }

        // The Sprites that are contained in the SpriteList
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<Sprite> Sprites
        {
            get
            {
                return _spriteList;
            }
        }

        // The Priority determines the order that the SpriteEngines are rendered
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                if (_surface != null)
                {
                    _surface.SortEngines();
                }
            }
        }

        // Should the engine detect collisions between its own sprites?
        public bool DetectCollisionSelf { get; set; }

        // The engine will detect collisions with other engines having the same tag
        public int DetectCollisionTag { get; set; }

        private void ThrowExceptionIfShapeless(Sprite sprite)
        {
            if (sprite.Shape.X == -1)
            {
                throw new InvalidOperationException("Sprite's Shape must be set before adding to SpriteEngine");
            }
        }

        // Return true if a new Sprite we wish to add will be in collision on being added.
        // If this is run on a Sprite that has been added, it will always return true.
        public bool WillCollide(Sprite sprite)
        {
            ThrowExceptionIfShapeless(sprite);
            for (int k = 0; k < Sprites.Count; k++)
            {
                Sprite other = Sprites[k];
                if (sprite.Bounds.IntersectsWith(other.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        // Add a sprite to the engine
        public void AddSprite(Sprite sprite)
        {
            ThrowExceptionIfShapeless(sprite);
            sprite._engine = this;
            sprite._surface = _surface;    
            lock (_spriteList)
            {
                _spriteList.Add(sprite);
            }            
        }

        // Remove a sprite from the engine
        public void RemoveSprite(Sprite sprite)
        {
            sprite.Kill();
        }

        // Remove all sprites
        public void Clear()
        {
            lock (_spriteList)
            {
                foreach (Sprite sprite in _spriteList)
                {
                    sprite.Kill();
                }
            }
        }

        // IComparable implementation - allows list of SpriteEngines to be sorted
        public int CompareTo(object obj)
        {
            SpriteEngine e2 = (SpriteEngine)obj;
            return e2.Priority - Priority;
        }

        // Internal methods - called by SpriteSurface
        internal void MoveSprites()
        {
            // Execute the protected "MoveSprite" method for each sprite we have
            lock (_spriteList)
            {
                foreach (Sprite sprite in Sprites)
                {
                    sprite.PreProcess();
                    sprite.launchProcess();
                    sprite.Mover?.MoveSprite(sprite);
                }
            }
        }

        // Wrap sprites around edges of surface if they are out of bounds
        internal void WrapSprites()
        {
            int Width = _surface.VirtualSize.Width;
            int Height = _surface.VirtualSize.Height;          
            lock (_spriteList)
            {
                foreach(Sprite sprite in Sprites)                 
                {
                    if (sprite.X < 0)
                        sprite.X = Width - 1;
                    else if (sprite.X >= Width)
                        sprite.X = 0;
                    if (sprite.Y < 0)
                        sprite.Y = Height - 1;
                    else if (sprite.Y >= Height)
                        sprite.Y = 0;
                }
            }
        }

        // Detect collisions among its own sprites
        internal void PerformSelfCollisionDetection()
        {
            lock (_spriteList)
            {
                for (int i = 0; i < Sprites.Count; i++)
                {
                    Sprite s1 = Sprites[i];
                    for (int k = i + 1; k < Sprites.Count; k++)
                    {
                        Sprite s2 = Sprites[k];
                        if (s1.Bounds.IntersectsWith(s2.Bounds))
                        {
                            _surface.TriggerCollision(s1, s2);
                        }
                    }
                }
            }
        }

        // Detect collisions against a different sprite engine
        internal void PerformCollisionWith(SpriteEngine se)
        {
            lock (_spriteList)
            {
                lock (se._spriteList)
                {
                    foreach (Sprite s1 in Sprites)
                    {
                        foreach (Sprite s2 in se.Sprites)
                        {
                            if (s1.Bounds.IntersectsWith(s2.Bounds))
                            {
                                _surface.TriggerCollision(s1, s2);
                            }
                        }
                    }
                }
            }
        }

        // Remove dead sprites from list
        internal void RemoveDeadSprites()
        {
            lock (_spriteList)
            {
                for (int i = 0; i < Sprites.Count; i++)
                {
                    Sprite s = Sprites[i];
                    if (s.Dead)
                    {
                        DeleteSprite(s);
                        SpriteRemoved?.Invoke(this, new SpriteEventArgs(s));
                    }
                }
            }
        }

        // Remove a sprite from the engine - called during processing cycle RemoveDeadSprites
        internal void DeleteSprite(Sprite sprite)
        {
            _spriteList.Remove(sprite);
            sprite._engine = null;
            sprite._surface = null;           
        }

    }
}
