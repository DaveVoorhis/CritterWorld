using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterWorld.Widgets
{
    // Based on https://stackoverflow.com/questions/778678/how-to-change-the-color-of-progressbar-in-c-sharp-net-3-5
    public class ColoredProgressBar : ProgressBar
    {
        public ColoredProgressBar()
        {
            SetStyle(ControlStyles.UserPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // None... Helps control the flicker.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Image offscreenImage = new Bitmap(Width, Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, Width, Height);
                    if (ProgressBarRenderer.IsSupported)
                    {
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);
                    }
                    rect.Width = (int)(rect.Width * ((double)Value / Maximum));
                    if (rect.Width == 0)
                    {
                        using (Pen pen = new Pen(Color.Black))
                        {
                            offscreen.DrawRectangle(pen, 0, 0, rect.Width, rect.Height);
                        }
                    }
                    else
                    {
                        using (LinearGradientBrush brush = new LinearGradientBrush(rect, BackColor, ForeColor, LinearGradientMode.Vertical))
                        {
                            offscreen.FillRectangle(brush, 0, 0, rect.Width, rect.Height);
                        }
                    }
                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                }
            }
        }
    }
}
