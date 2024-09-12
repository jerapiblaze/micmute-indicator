using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace micmute_indicator.Helpers
{
    internal class MuteForm : OSDNativeForm
    {
        protected static string osdText = "Mic muted";
        protected static Bitmap icon = Properties.Resources.icons8_mute;

        public MuteForm()
        {
            
        }

        protected override void PerformPaint(PaintEventArgs e)
        {
            Brush brush = new SolidBrush(Color.FromArgb(150, Color.Black));

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.FillRoundedRectangle(brush, Bound, 10);
            e.Graphics.FillRectangle(brush, Bound);

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;

            int shiftX = 0;

            if (icon is not null)
            {
                e.Graphics.DrawImage(icon, 9, 9, 32, 32);
                shiftX = 20;
            }

            e.Graphics.DrawString(osdText,
                new Font("Segoe UI", 18f, FontStyle.Bold, GraphicsUnit.Pixel),
                new SolidBrush(Color.White),
                new PointF(Bound.Width / 2 + shiftX, Bound.Height / 2),
            format);

        }

        public void Display()
        {
            Screen screen1 = Screen.FromHandle(Handle);
            Width = 150;
            Height = 50;
            X = (screen1.Bounds.Width - Width) * 11/12;
            Y = (screen1.Bounds.Height - Height) * 14/15;

            Show();
        }
    }
}
