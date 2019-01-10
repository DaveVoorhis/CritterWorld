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
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace SCG.TurboSprite
{
    // Implements an animated surface where sprites can be registered
    public partial class SpriteSurface : Control
    {
        private int _desiredFPS = 10;
        private bool _active;
        private Thread _thrdAnimate;       
        private int _lastSecond = -1;
        private int _compareSecond;
        private DateTime _dtStamp;
        private int _frames = 0;
        private DateTime _nextFrameTime;
        private TimeSpan _animationSpan = new TimeSpan(0, 0, 0, 0, 100);
        private Bitmap _buffer = new Bitmap(1, 1);
        private List<SpriteEngine> _engineList = new List<SpriteEngine>();
        private Size _virtualSize = new Size();
        private int _offsetX;
        private int _offsetY;

        public event EventHandler<EventArgs> BeforeAnimationCycle;
        public event EventHandler<PaintEventArgs> BeforeSpriteRender;
        public event EventHandler<PaintEventArgs> AfterSpriteRender;
        public event EventHandler<SpriteCollisionEventArgs> SpriteCollision;
        public event EventHandler<SpriteClickEventArgs> SpriteClicked;

        public SpriteSurface()
        {
            InitializeComponent();
        }

        public SpriteSurface(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        // The frames per second desired by the user
        public int DesiredFPS
        {
            get
            {
                return _desiredFPS;
            }
            set
            {
                if (value >= 1 && value <= 1000)
                {
                    _desiredFPS = value;
                    _animationSpan = new TimeSpan(0, 0, 0, 0, (1000 / value));                 
                }
            }
        }

        // The actual frames per second the control is animating at
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ActualFPS { get; private set; }

        // "Active" property turns animation on and off
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                _active = value;
                if (Active)
                {
                    // Create animation thread, set to background to app to close while running
                    _thrdAnimate = new Thread(AnimateProc)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority
                    };

                    // We need an animation update right away
                    _nextFrameTime = DateTime.Now;

                    // Activate thread
                    _thrdAnimate.Start();
                }
                else
                {
                    _lastSecond = -1;
                }
            }
        }

        // Priority of the animation thread
        public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.Normal;

        // AutoBlank property determines if background will be filled before rendering frame
        public bool AutoBlank { get; set; }

        // Color to fill for AutoBlank
        public Color AutoBlankColor { get; set; }

        // Should sprites wrap around the edge if they move beyond it?
        public bool WraparoundEdges { get; set; }

        // Virtual size
        public bool UseVirtualSize { get; set; }

        public Size VirtualSize
        {
            get
            {
                if (UseVirtualSize)
                {
                    return _virtualSize;
                }
                else
                {
                    return Size;
                }
            }
            set
            {              
                _virtualSize = value;
            }
        }

        [Browsable(false)]
        public int VirtualWidth
        {
            get
            {
                return _virtualSize.Width;
            }
            set
            {
                _virtualSize.Width = value;
            }
        }

        [Browsable(false)]
        public int VirtualHeight
        {
            get
            {
                return _virtualSize.Height;
            }
            set
            {
                _virtualSize.Height = value;
            }
        }

        // Offset into virtual size
        public int OffsetX
        {
            get
            {
                return _offsetX;
            }
            set
            {
                if (value > 0 && value < VirtualWidth - Width)
                {
                    _offsetX = value;
                }
            }
        }

        public int OffsetY
        {
            get
            {
                return _offsetY;
            }
            set
            {
                if (value > 0 && value < VirtualHeight - Height)
                {
                    _offsetY = value;
                }
            }
        }

        // Center on the specific coordinates
        public void CenterOn(int x, int y)
        {
            if (!UseVirtualSize)
            {
                return;
            }
            int centerx = x - Width / 2;
            int centery = y - Height / 2;
            if (centerx < 0)
            {
                centerx = 0;
            }
            if (centery < 0)
            {
                centery = 0;
            }
            OffsetX = centerx;
            OffsetY = centery;
        }

        // Thread animation procedure
        protected void AnimateProc()
        {            
            while (Active)
            {
                // Do we need to render a frame?
                if (DateTime.Now > _nextFrameTime)
                {
                    // Determine time stamp that next frame needs to be rendered
                    _nextFrameTime = DateTime.Now + _animationSpan;

                    // Trigger event to client
                    BeforeAnimationCycle?.Invoke(this, EventArgs.Empty);

                    // Process Sprite movement
                    lock (_engineList)
                    {
                        for (int i = 0; i < _engineList.Count; i++)
                        {
                            SpriteEngine se = _engineList[i];
                            se.MoveSprites();
                            // Handle wrapping around edges
                            if (WraparoundEdges)
                            {
                                se.WrapSprites();
                            }
                            // Handle collision detection with itself
                            if (se.DetectCollisionSelf && SpriteCollision != null)
                            {
                                se.PerformSelfCollisionDetection();
                            }
                            // Handle collision detection with other engines
                            for (int k = i + 1; k < _engineList.Count; k++)
                            {
                                SpriteEngine se2 = _engineList[k];
                                if (se.DetectCollisionTag == se2.DetectCollisionTag)
                                {
                                    se.PerformCollisionWith(se2);
                                }
                            }
                        }
                        // Remove dead sprites from engines
                        foreach (SpriteEngine se in _engineList)
                        {
                            se.RemoveDeadSprites();
                        }
                    }
                    // Trigger a repaint of surface
                    Invalidate();
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        // Override to avoid flickering when animating
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!Active)
            {
                base.OnPaintBackground(pevent);
            }
        }

        // Paint procedure
        protected override void OnPaint(PaintEventArgs e)
        {
            // See if we need to re-size the double buffer
            if (_buffer.Size != Size)
            {
                _buffer = new Bitmap(Width, Height);
            }

            try
            {
                // Obtain Graphics object of double buffer
                Graphics grBuffer = Graphics.FromImage(_buffer);

                // Make pretty
                grBuffer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Obtain Graphics object of SpriteSurface
                Graphics grSurface = e.Graphics;
                using (grBuffer)
                {
                    // Process AutoBlank
                    if (AutoBlank)
                    {
                        grBuffer.Clear(AutoBlankColor);
                    }

                    // Allow client to hook in before sprites are rendered
                    BeforeSpriteRender?.Invoke(this, new PaintEventArgs(grBuffer, e.ClipRectangle));

                    // now give derived classes a chance
                    DoBeforeSpriteRender(grBuffer);

                    // Render Sprites
                    Rectangle viewport = new Rectangle(OffsetX, OffsetY, Width, Height);
                    lock (_engineList)
                    {
                        foreach (SpriteEngine se in _engineList)
                        {
                            lock (se._spriteList)
                            {
                                foreach (Sprite sprite in se.Sprites)
                                {
                                    if (sprite.Bounds.IntersectsWith(viewport))
                                    {
                                        sprite.Render(grBuffer);
                                    }
                                }
                            }
                        }
                    }

                    // Allow client to hook in after sprites are rendered
                    AfterSpriteRender?.Invoke(this, new PaintEventArgs(grBuffer, e.ClipRectangle));

                    DoAfterSpriteRender(grBuffer);

                    // Copy double buffer to primary Graphics object
                    grSurface.DrawImage(_buffer, 0, 0);

                    // Has another second worth or animation time elapsed?
                    _dtStamp = DateTime.Now;
                    _compareSecond = _dtStamp.Second;
                    if (_compareSecond != _lastSecond)
                    {
                        // Yes, update the number of actual frames per second we animated
                        if (_lastSecond != -1)
                            ActualFPS = _frames;
                        _frames = 1;
                        _lastSecond = _compareSecond;
                    }
                    else
                    {
                        _frames++;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception during rendering: " + exception);
            }
            finally
            {
                // Allow custom OnPaint handler to execute
                base.OnPaint(e);
            }
        }

        // Process clicking of sprite
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (SpriteClicked == null)
            {
                return;
            }
            Point pt = new Point(e.X, e.Y);
            foreach (SpriteEngine engine in _engineList)
            {
                lock (engine._spriteList)
                {
                    foreach (Sprite sprite in engine.Sprites)
                    {
                        if (sprite.ClickBounds.Contains(pt))
                        {
                            SpriteClicked(this, new SpriteClickEventArgs(sprite, e.Button));
                        }
                    }
                }
            }
        }

        // Register and de-register SpriteEngines - called by the SpriteEngine.Surface settor
        internal void RegisterSpriteEngine(SpriteEngine engine)
        {
            lock (_engineList)
            {
                _engineList.Add(engine);
                SortEngines();
            }
        }

        internal void UnRegisterSpriteEngine(SpriteEngine engine)
        {
            lock (_engineList)
            {
                _engineList.Remove(engine);
            }
        }

        // Sort the SpriteEngine list, after change of priority, or newly added
        internal void SortEngines()
        {
            _engineList.Sort();
        }

        // Trigger collision detection event - called from SpriteEngine collision detection
        internal void TriggerCollision(Sprite sprite1, Sprite sprite2)
        {
            SpriteCollision?.Invoke(this, new SpriteCollisionEventArgs(sprite1, sprite2));
        }

        // Protected methods provide hooks to derived classes
        protected virtual void DoBeforeSpriteRender(Graphics g)
        {
        }

        protected virtual void DoAfterSpriteRender(Graphics g)
        {
        }
    }
}
