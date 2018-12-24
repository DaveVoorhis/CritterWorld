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
//Provides the ability to generate Bitmaps based on a single cell-based master Bitmap
//The master Bitmap is divided into a number of cells across, each corresponding to a different sprite that should be drawn
//Additionally, the factory can return colored versions of the Bitmaps - the source should be a grayscale Bitmap

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing;

namespace SCG.TurboSprite
{
    public partial class GamePieceBitmapFactory : Component
    {
        private Bitmap _masterBitmap;
        private int _cellsX;
        private int _cellsY;
        private Dictionary<Color, Bitmap> _colorized = new Dictionary<Color, Bitmap>();
        private Dictionary<Bitmap, Bitmap[,]> _mapping = new Dictionary<Bitmap, Bitmap[,]>();
        private Bitmap[,] _cells = null;
        private Dictionary<RotatedBitmap, Bitmap> _rotations = new Dictionary<RotatedBitmap, Bitmap>();

        // Constructors
        public GamePieceBitmapFactory()
        {
            InitializeComponent();
        }

        public GamePieceBitmapFactory(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        // The Master bitmap that contains all of the cells
        public Bitmap MasterBitmap
        {
            get
            {
                return _masterBitmap;
            }
            set
            {
                _masterBitmap = value;
            }
        }

        // The number of cells that the master bitmap contains
        public int CellsX
        {
            get
            {
                return _cellsX;
            }
            set
            {
                _cellsX = value;
            }
        }

        public int CellsY
        {
            get
            {
                return _cellsY;
            }
            set
            {
                _cellsY = value;
            }
        }

        // Returns the cell width/height
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellWidth
        {
            get
            {
                if (_cellsX == 0 || _masterBitmap == null)
                    return 0;
                return _masterBitmap.Width / _cellsX;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CellHeight
        {
            get
            {
                if (_cellsY == 0 || _masterBitmap == null)
                    return 0;
                return _masterBitmap.Height / _cellsY;
            }
        }

        // Return a bitmap based on cell, do not colorize
        public Bitmap GetGamePieceBitmap(int cellx, int celly)
        {
            if (_cells == null)
            {
                // break up the bitmap into cells
                Bitmap copied = new Bitmap(_masterBitmap.Width, _masterBitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(copied);
                using (g)
                {
                    g.DrawImage(_masterBitmap, new Rectangle(0, 0, _masterBitmap.Width, _masterBitmap.Height));
                }

                _cells = new Bitmap[CellsX, CellsY];
                for (int y = 0; y < CellsY; y++)
                    for (int x = 0; x < CellsX; x++)
                    {
                        Bitmap cellBitmap = new Bitmap(CellWidth, CellHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        Graphics cellGraphics = Graphics.FromImage(cellBitmap);
                        using (cellGraphics)
                        {
                            cellGraphics.DrawImage(copied, 0, 0, new Rectangle(CellWidth * x, CellHeight * y, CellWidth, CellHeight), GraphicsUnit.Pixel);
                        }
                        _cells[x, y] = cellBitmap;
                    }                
            }
            return _cells[cellx, celly];
        }

        // Return a Bitmap based on one of the cells, with the specified Color
        public Bitmap GetGamePieceBitmap(int cellx, int celly, Color color)
        {
            // Have we already created a colorized version of the master bitmap?
            if (!_colorized.ContainsKey(color))
            {
                // Create colorized version of the source
                Color c;
                float red = color.R;
                float green = color.G;
                float blue = color.B;
                byte newRed, newGreen, newBlue;
                float pct;
                Bitmap colorized = new Bitmap(_masterBitmap.Width, _masterBitmap.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(colorized);
                using (g)
                {
                    g.DrawImage(_masterBitmap, new Rectangle(0, 0, _masterBitmap.Width, _masterBitmap.Height));
                }
                for (int x = 0; x < colorized.Width; x++)
                    for (int y = 0; y < colorized.Height; y++)
                    {
                        c = colorized.GetPixel(x, y);
                        if (c != Color.Black)
                        {
                            pct = (float)c.R / 255f;
                            newRed = (byte)(red * pct);
                            newGreen = (byte)(green * pct);
                            newBlue = (byte)(blue * pct);
                            colorized.SetPixel(x, y, Color.FromArgb(newRed, newGreen, newBlue));
                        }
                    }

                // Save colorized source
                _colorized.Add(color, colorized);

                // Chop colorized source into array of cells
                Bitmap[,] cells = new Bitmap[CellsX, CellsY];
                for(int y = 0; y < CellsY; y++)
                    for (int x = 0; x < CellsX; x++)
                    {
                        Bitmap cellBitmap = new Bitmap(CellWidth, CellHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                        Graphics cellGraphics = Graphics.FromImage(cellBitmap);
                        using (cellGraphics)
                        {
                            cellGraphics.DrawImage(colorized, 0, 0, new Rectangle(CellWidth * x, CellHeight * y, CellWidth, CellHeight), GraphicsUnit.Pixel);
                        }
                        cells[x, y] = cellBitmap;
                    }

                // Save the cell array for reference
                _mapping.Add(colorized, cells);
            }

            // Find the colorized bitmap
            Bitmap colorizedMaster = _colorized[color];

            // Find the array of cells
            Bitmap[,] cellArray = _mapping[colorizedMaster];

            // Return the appropriate cell
            return cellArray[cellx, celly];
        }

        // Get a rotated bitmap
        public Bitmap GetGamePieceBitmap(int cellx, int celly, Color color, Rotation rotation)
        {
            // Get the base (unrotated) cell
            Bitmap bmp = GetGamePieceBitmap(cellx, celly, color);

            // If we have one already return it
            RotatedBitmap rb = new RotatedBitmap(bmp, rotation);
            if (_rotations.ContainsKey(rb))
                return _rotations[rb];

            // create it
            Bitmap rotatedBmp = new Bitmap(bmp);
            switch (rotation)
            {
                case Rotation.R90:
                    rotatedBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Rotation.R180:
                    rotatedBmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Rotation.R270:
                    rotatedBmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            _rotations[rb] = rotatedBmp;
            return rotatedBmp;
        }
    }

    // Supported rotations
    public enum Rotation { R90, R180, R270 };

    // Desired Bitmap/Rotation combo
    internal struct RotatedBitmap
    {
        internal RotatedBitmap(Bitmap bitmap, Rotation rotation)
        {
            Bitmap = bitmap;
            Rotation = rotation;
        }

        internal Bitmap Bitmap;
        internal Rotation Rotation;
    }
}
