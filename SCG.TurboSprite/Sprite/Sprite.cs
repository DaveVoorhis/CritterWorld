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
    //Sprite class - defines behavior of all TurboSprite sprite objects
    public abstract class Sprite
    {
        private int _facingAngle;
        private RectangleF _shape = new RectangleF(-1, -1, -1, -1);
        private RectangleF _bounds = new RectangleF();
        private RectangleF _clickBounds = new RectangleF();

        public event EventHandler<SpriteEventArgs> Died;

        internal SpriteEngine _engine;
        internal SpriteSurface _surface;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        // sin/cos tables
        private static readonly float[] _sin = new float[360];
        private static readonly float[] _cos = new float[360];

        // Static constructor populates the sin/cos lookup tables
        static Sprite()
        {
            for (int degree = 0; degree < 360; degree++)
            {
                _sin[degree] = (float)Math.Sin(DegToRad(degree));
                _cos[degree] = (float)Math.Cos(DegToRad(degree));
            }
        }

        // Get a random color byte value
        private static byte RndByte(byte b1, byte b2)
        {
            if (b1 > b2)
            {
                return (byte)(rnd.Next(b1 - b2) + b2);
            }
            return (byte)(rnd.Next(b2 - b1) + b1);
        }

        // Obtain a random color within start to end range
        public static Color RandomColorFromRange(Color startColor, Color endColor)
        {
            byte a = RndByte(startColor.A, endColor.A);
            byte r = RndByte(startColor.R, endColor.R);
            byte g = RndByte(startColor.G, endColor.G);
            byte b = RndByte(startColor.B, endColor.B);
            return Color.FromArgb(a, r, g, b);
        }

        // Obtain a random color given a minimum luminance value.
        public static Color RandomColor(int minimumLuminance)
        {
            byte r = (byte)rnd.Next(minimumLuminance, 256);
            byte g = (byte)rnd.Next(minimumLuminance, 256);
            byte b = (byte)rnd.Next(minimumLuminance, 256);
            return Color.FromArgb(255, r, g, b);
        }

        // Distance between a pair of points.
        public static double GetDistance(float x1, float y1, float x2, float y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        // Convert speed to direction in degrees.
        public static float GetAngle(float speedX, float speedY)
        {
            return RadToDeg((float)Math.Atan2(speedY, speedX));
        }

        // Utility function to convert degrees to radians.
        public static float DegToRad(float degree)
        {
            return (float)(Math.PI / 180 * degree);
        }

        // Utility function to convert radians to degrees.
        public static float RadToDeg(float rad)
        {
            return (float)(rad * 180 / Math.PI);
        }

        // Given an arbitrary angle in degrees, return it normalised to 0 - 359
        public static int NormaliseAngle(int angle)
        {
            int tmp = angle % 360;
            return (tmp < 0) ? angle + 360 : tmp;
        }

        // Static properties return the Sin/Cos for specified degree values
        public static float Sin(int degree)
        {
            return _sin[degree];
        }

        public static float Cos(int degree)
        {
            return _cos[degree];
        }

        // This can be used to associate user data with the Sprite.
        public object Data
        {
            get; set;
        }

        // The "Shape" of the sprite represents its Width and Height as relative to its center
        public RectangleF Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
                ClickShape = value;
            }
        }

        // Clickshape determines the size of the sprite for purposes of registering a mouse click
        public RectangleF ClickShape { get; set; } = new RectangleF(-1, -1, -1, -1);

        // Sprite's bounding rectangle, calculated based on size and position
        public RectangleF Bounds
        {
            get
            {
                _bounds.X = X + _shape.Left;
                _bounds.Width = _shape.Width;
                _bounds.Y = Y + _shape.Top;
                _bounds.Height = _shape.Height;
                return _bounds;
            }
        }

        // Bounding rectangle of clickable region
        public RectangleF ClickBounds
        {
            get
            {
                _clickBounds.X = X + ClickShape.Left - Surface.OffsetX;
                _clickBounds.Width = ClickShape.Width;
                _clickBounds.Y = Y + ClickShape.Top - Surface.OffsetY;
                _clickBounds.Height = ClickShape.Height;
                return _clickBounds;
            }
        }

        // Helper property, returns integer Width and Height
        public int Width
        {
            get
            {
                return (int)Shape.Width;
            }
        }

        public int Height
        {
            get
            {
                return (int)Shape.Height;
            }
        }

        // Helper properties, returns half of the Width and Height
        public int WidthHalf
        {
            get
            {
                return Width / 2;
            }
        }

        public int HeightHalf
        {
            get
            {
                return Height / 2;
            }
        }

        // Angle sprite is facing - values between 0 and 360 allowed - auto-conversion occurs
        public int FacingAngle
        {
            get
            {
                return _facingAngle;
            }
            set
            {
                _facingAngle = Sprite.NormaliseAngle(value);
            }
        }

        // The Sprite's associated SpriteEngine
        public SpriteEngine Engine
        {
            get
            {
                return _engine;
            }
        }

        // Expose the surface
        public SpriteSurface Surface
        {
            get
            {
                return _surface;
            }
        }

        // Is the sprite dead?
        public bool Dead { get; private set; }

        // Sprite's position - integer and float types supported
        public float X { get; set; }

        public float Y { get; set; }

        public PointF PositionF
        {
            get
            {
                return new PointF(X, Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public Point Position
        {
            get
            {
                return new Point((int)X, (int)Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        // The sprite's spin (if any)
        public SpinType Spin { get; set; }

        public int SpinSpeed { get; set; }

        // The sprite's mover.
        public IMover Mover { get; set; }

        // Kill a sprite - it will be removed after next processing cycle
        public virtual void Kill()
        {
            Died?.Invoke(this, new SpriteEventArgs(this));
            Dead = true;
        }

        // Process the internal logic a sprite may require during each animation cycle
        internal void PreProcess()
        {
            switch (Spin)
            {
                case SpinType.Clockwise:
                    FacingAngle += SpinSpeed;
                    break;
                case SpinType.CounterClockwise:
                    FacingAngle -= SpinSpeed;
                    break;
            }
        }

        // Render the sprite on the SpriteSurface
        protected internal abstract void Render(Graphics g);

        public delegate void Processor(Sprite sprite);

        public event Processor Processors;

        // Launch any additional processing.
        protected internal void LaunchProcess()
        {
            Processors?.Invoke(this);
        }
    }

    // Direction of sprite's spin
    public enum SpinType { None, Clockwise, CounterClockwise };
}
