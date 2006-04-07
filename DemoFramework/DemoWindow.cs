using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DemoFramework
{
    public class DemoWindow : Form
    {
        public enum Aspect
        {
            ASPECT_INVALID,
            ASPECT_4_3,
            ASPECT_16_9
        }

        public Aspect AspectRatio
        {
            get
            {
                if (ClientSize.Width * 3 / 4 == ClientSize.Height)
                    return Aspect.ASPECT_4_3;
                if (ClientSize.Width * 9 / 16 == ClientSize.Height)
                    return Aspect.ASPECT_16_9;
                return Aspect.ASPECT_INVALID;
            }
        }

        public DemoWindow()
        {
        }

        internal void Initialize(int width, int height, string name)
        {
            ClientSize = new Size(width, height);
            if (AspectRatio == Aspect.ASPECT_INVALID)
                throw new ArgumentOutOfRangeException("Width, Height", "Window dimensions must have an spect ratio of 4:3 or 16:9");

            this.Text = name;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            Cursor.Hide();
            Cursor.Dispose();
            Show();
        }
    }
}
