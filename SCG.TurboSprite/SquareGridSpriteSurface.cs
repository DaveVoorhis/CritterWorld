#region copyright
/*
* Copyright (c) 2008, Dion Kurczek
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
using System.Windows.Forms;

namespace SCG.TurboSprite
{
    public partial class SquareGridSpriteSurface : SpriteSurface
    {
        //Events
        public event EventHandler<CellEventArgs> CellClicked;
        public event EventHandler<RangeSelectedEventArgs> RangeSelected;

        //Constructors
        public SquareGridSpriteSurface()
        {
            InitializeComponent();
            UseVirtualSize = true;
        }

        public SquareGridSpriteSurface(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            UseVirtualSize = true;
        }

        //Public properties
        public int CellWidth
        {
            get
            {
                return _cellWidth;
            }
            set
            {
                if (value > 0)
                {
                    _cellWidth = value;
                    VirtualWidth = _cellWidth * _cellsX;
                }
            }
        }

        public int CellHeight
        {
            get
            {
                return _cellHeight;
            }
            set
            {
                if (value > 0)
                {
                    _cellHeight = value;
                    VirtualHeight = _cellHeight * _cellsY;
                }
            }
        }

        public int CellsX
        {
            get
            {
                return _cellsX;
            }
            set
            {
                if (value > 0)
                {
                    _cellsX = value;
                    VirtualWidth = _cellsX * _cellWidth;
                }
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
                if (value > 0)
                {
                    _cellsY = value;
                    VirtualHeight = _cellsY * _cellHeight;
                }
            }
        }

        //Shold the grid be drawn?
        public bool GridVisible
        {
            get
            {
                return _drawGrid;
            }
            set
            {
                _drawGrid = value;
            }
        }

        public Color GridColor
        {
            get
            {
                return _gridColor;
            }
            set
            {
                _gridColor = value;
            }
        }

        //Control offset via grid
        public int OffsetCellX
        {
            get
            {
                return _offsetCellX;
            }
            set
            {
                if (value >= 0 && value < CellsX - VisibleCellsX)
                {
                    _offsetCellX = value;
                    OffsetX = value * _cellWidth;
                }
            }
        }
        public int OffsetCellY
        {
            get
            {
                return _offsetCellY;
            }
            set
            {
                if (value >= 0 && value < CellsY - VisibleCellsY)
                {
                    _offsetCellY = value;
                    OffsetY = value * _cellHeight;
                }
            }
        }

        //Number of visible cells
        public int VisibleCellsX
        {
            get
            {
                return Width / CellWidth;
            }
        }

        public int VisibleCellsY
        {
            get
            {
                return Height / CellHeight;
            }
        }

        //Color of cursor
        public Color CursorColor
        {
            get
            {
                return _cursorColor;
            }
            set
            {
                _cursorColor = value;
                _penCursor = new Pen(_cursorColor, _cursorWidth);
            }
        }

        //Position of cursor
        public int CursorX
        {
            get
            {
                return _cursorX;
            }
            set
            {
                _cursorX = value;
            }
        }

        public int CursorY
        {
            get
            {
                return _cursorY;
            }
            set
            {
                _cursorY = value;
            }
        }

        //Should we draw the cursor?
        public bool CursorVisible
        {
            get
            {
                return _cursorVisible;
            }
            set
            {
                _cursorVisible = value;
            }
        }

        //Width of cursor
        public int CursorWidth
        {
            get
            {
                return _cursorWidth;
            }
            set
            {
                _cursorWidth = value;
                _penCursor = new Pen(_cursorColor, _cursorWidth);
            }
        }

        //center on a specified cell
        public void CenterOnCell(int x, int y)
        {
            int cellsWide = Width / CellWidth;
            int cellsHigh = Height / CellHeight;
            int leftCell = x - cellsWide / 2;
            int topCell = y - cellsHigh / 2;
            if (leftCell < 0)
                leftCell = 0;
            if (topCell < 0)
                topCell = 0;
            if (leftCell > CellsX - cellsWide)
                leftCell = CellsX - cellsWide;
            if (topCell > CellsY - cellsHigh)
                topCell = CellsY - cellsHigh;
            OffsetCellX = leftCell;
            OffsetCellY = topCell;
        }

        //Should there be a selection band?
        public bool SelectionBand
        {
            get
            {
                return _selectionBand;
            }
            set
            {
                _selectionBand = value;
            }
        }

        //Color of the selection band
        public Color SelectionBandColor
        {
            get
            {
                return _selectionBandColor;
            }
            set
            {
                _selectionBandColor = value;
                _penSelectionBand = new Pen(value, 2);
            }
        }

        //Private members
        private int _cellWidth;
        private int _cellHeight;
        private int _cellsX;
        private int _cellsY;
        private bool _drawGrid;
        private Color _gridColor;
        private int _offsetCellX;
        private int _offsetCellY;
        private Color _cursorColor;
        private int _cursorX;
        private int _cursorY;
        private bool _cursorVisible;
        private Pen _penCursor;
        private int _cursorWidth = 4;
        private bool _selectionBand = false;
        private Color _selectionBandColor = Color.Aqua;
        private bool _dragging = false;
        private Rectangle _dragRect = new Rectangle(0, 0, 0, 0);
        private int _startX = 0;
        private int _startY = 0;
        private int _dragX = 0;
        private int _dragY = 0;
        private Pen _penSelectionBand = Pens.Aqua;

        //Render the grid before sprites are drawn
        protected override void DoBeforeSpriteRender(System.Drawing.Graphics g)
        {
            if (GridVisible)
            {
                using (Pen p = new Pen(GridColor))
                {
                    int x_, y_;
                    for (int x = 0; x < VirtualWidth; x += _cellWidth)
                    {
                        x_ = x - OffsetX;
                        if (x_ >= 0 && x_ <= Width)
                        {
                            g.DrawLine(p, x_, 0, x_, Height);
                        }
                    }
                    for (int y = 0; y < VirtualHeight; y += _cellHeight)
                    {
                        y_ = y - OffsetY;
                        if (y_ >= 0 && y_ <= Height)
                        {
                            g.DrawLine(p, 0, y_, Width, y_);
                        }
                    }
                }
            }
        }

        //Render the cursor
        protected override void DoAfterSpriteRender(Graphics g)
        {
            //Draw the cursor
            if (CursorVisible)
            {
                int x = _cursorX * CellWidth - OffsetX;
                int y = _cursorY * CellHeight - OffsetY;
                g.DrawRectangle(_penCursor, x, y, CellWidth, CellHeight);
            }

            //Draw the selection band
            if (SelectionBand && _dragging)
            {
                int x = _startX < _dragX ? _startX : _dragX;
                int y = _startY < _dragY ? _startY : _dragY;
                x = x * CellWidth - OffsetX;
                y = y * CellHeight - OffsetY;
                int w = Math.Abs(_startX - _dragX) * CellWidth + CellWidth;
                int h = Math.Abs(_startY - _dragY) * CellHeight + CellHeight;
                g.DrawRectangle(_penSelectionBand, x, y, w, h);
            }
        }

        //Sense a drag
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _dragging = true;
            _startX = (e.X + OffsetX) / CellWidth;
            _startY = (e.Y + OffsetY) / CellHeight;
            _dragX = _startX;
            _dragY = _startY;
        }

        //Respond to cell click
        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool range = false;

            //get coordinates
            int x = (e.X + OffsetX) / CellWidth;
            int y = (e.Y + OffsetY) / CellHeight;

            //Process a drag?
            _dragging = false;
            if ((x != _startX || y != _startY) && SelectionBand)
            {
                if (x > _startX)
                {
                    _dragRect.X = _startX;
                    _dragRect.Width = x - _startX + 1;
                }
                else
                {
                    _dragRect.X = x;
                    _dragRect.Width = _startX - x + 1;
                }
                if (y > _startY)
                {
                    _dragRect.Y = _startY;
                    _dragRect.Height = y - _startY + 1;
                }
                else
                {
                    _dragRect.Y = y;
                    _dragRect.Height = _startY - y + 1;
                }
                if (RangeSelected != null)
                    RangeSelected(this, new RangeSelectedEventArgs(_dragRect));
                range = true;
            }

            base.OnMouseUp(e);

            if (!range)
                if (CellClicked != null)
                    CellClicked(this, new CellEventArgs(x, y, e.Button));            
        }

        //if dragging update end coords
        protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging)
            {
                _dragX = (e.X + OffsetX) / CellWidth;
                _dragY = (e.Y + OffsetY) / CellHeight;
            }
        }
    }

    //Event triggered with cell 
    public class CellEventArgs : EventArgs
    {
        //public members
        public CellEventArgs(int x, int y, MouseButtons mb)
        {
            _x = x;
            _y = y;
            _mb = mb;
        }

        //Retrive coordinates
        public int X
        {
            get
            {
                return _x;
            }
        }
        public int Y
        {
            get
            {
                return _y;
            }
        }
        public MouseButtons Button
        {
            get
            {
                return _mb;
            }
        }

        //private members
        private int _x;
        private int _y;
        private MouseButtons _mb;
    }

    //event that's triggered when a range is selected
    public class RangeSelectedEventArgs : EventArgs
    {
        //constructor
        public RangeSelectedEventArgs(Rectangle rect)
        {
            _selection = rect;
        }

        //access the selected range
        public Rectangle SelectedRange
        {
            get
            {
                return _selection;
            }
        }

        //private members
        private Rectangle _selection;
    }
}
