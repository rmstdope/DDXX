using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Direct3D;
using Input;
using Utility;

namespace DemoFramework
{
    public class DemoWindow : Form
    {
        public AspectRatio AspectRatio
        {
            get
            {
                return new AspectRatio(ClientSize.Width, ClientSize.Height); 
            }
        }

        public DemoWindow()
        {
        }

        public void Initialize(string name, DeviceDescription desc)
        {
            D3DDriver gDriver = D3DDriver.GetInstance();
            gDriver.Initialize(this, desc);

            InputDriver iDriver = InputDriver.GetInstance();
            iDriver.Initialize(this);

            ClientSize = new Size(desc.width, desc.height);
            if (AspectRatio.Ratios.RATIO_INVALID == new AspectRatio(ClientSize.Width, ClientSize.Height).Ratio)
                throw new DDXXException("Width and height of window does not match any valid aspect ratio.");

            this.Text = name;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            Cursor.Hide();
            Cursor.Dispose();
            Show();
        }
    }
}
