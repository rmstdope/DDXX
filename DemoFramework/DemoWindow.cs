using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
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
            ClientSize = new Size(desc.width, desc.height);
            if (AspectRatio.Ratios.RATIO_INVALID == new AspectRatio(ClientSize.Width, ClientSize.Height).Ratio)
                throw new DDXXException("Width and height of window does not match any valid aspect ratio.");

            this.Text = name;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);

            Cursor.Hide();
            Cursor.Dispose();

            D3DDriver gDriver = D3DDriver.GetInstance();
            gDriver.Initialize(this, desc);

            InputDriver iDriver = InputDriver.GetInstance();
            iDriver.Initialize(this);

            Debug.WriteLine(gDriver.GetDevice().PresentationParameters.ToString());
            
            Show();
        }

        public void CleanUp()
        {
            D3DDriver gDriver = D3DDriver.GetInstance();
            gDriver.GetDevice().Dispose();
        }
    }
}
