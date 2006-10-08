using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweaker : IDemoTweaker
    {
        private bool enabled;
        private IUserInterface userInterface;

        private BoxControl mainWindow;
        private BoxControl titleWindow;
        private TextControl titleText;
        private BoxControl timeWindow;

        private float alpha = 0.4f;
        private Color titleColor = Color.Aquamarine;
        private Color timeColor = Color.BurlyWood;

        public IUserInterface UserInterface
        {
            set { userInterface = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public DemoTweaker()
        {
            enabled = false;
            userInterface = new UserInterface();
        }

        public void Initialize()
        {
            userInterface.Initialize();

            mainWindow = new BoxControl(new RectangleF(0.05f, 0.05f, 0.90f, 0.90f), 0.0f, Color.Black, null);
            titleWindow = new BoxControl(new RectangleF(0.0f, 0.0f, 1.0f, 0.05f), alpha, titleColor, mainWindow);
            titleText = new TextControl("DDXX Tweaker", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, alpha, Color.White, titleWindow);
            timeWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f), alpha, timeColor, mainWindow);
        }

        public void Draw()
        {
            if (!Enabled)
                return;

            D3DDriver.GetInstance().GetDevice().BeginScene();
            userInterface.DrawControl(mainWindow);
            D3DDriver.GetInstance().GetDevice().EndScene();
        }
    }
}
