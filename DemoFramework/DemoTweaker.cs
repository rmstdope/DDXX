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
        private IDemoRegistrator registrator;

        private BoxControl mainWindow;
        private BoxControl titleWindow;
        private TextControl titleText;
        private BoxControl timeWindow;

        private float alpha = 0.4f;
        private float textAlpha = 0.6f;
        private Color titleColor = Color.Aquamarine;
        private Color timeColor = Color.BurlyWood;
        private Color selectedTrackColor = Color.Crimson;
        private Color trackColor = Color.DarkBlue;

        private int currentTrack;
        private float startTime;
        private float timeScale;

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
            currentTrack = 0;
            startTime = 0;
            timeScale = 10.0f;
        }

        public void Initialize(IDemoRegistrator registrator)
        {
            this.registrator = registrator;
            startTime = registrator.StartTime;

            userInterface.Initialize();

            CreateControls();
        }

        private void CreateControls()
        {
            mainWindow = new BoxControl(new RectangleF(0.05f, 0.05f, 0.90f, 0.90f), 0.0f, Color.Black, null);

            titleWindow = new BoxControl(new RectangleF(0.0f, 0.0f, 1.0f, 0.05f), alpha, titleColor, mainWindow);
            titleText = new TextControl("DDXX Tweaker", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, textAlpha, Color.White, titleWindow);

            timeWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f), alpha, timeColor, mainWindow);
        }

        public void Draw()
        {
            if (!Enabled)
                return;



            Color color;
            float y = 0.05f;
            for (int i = currentTrack - 1; i < currentTrack + 2; i++)
            {
                if (i < 0 || i >= registrator.Tracks.Count)
                    continue;
                if (i == currentTrack)
                    color = selectedTrackColor;
                else
                    color = trackColor;
                BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.25f), alpha, color, timeWindow);
                new TextControl("Track " + i, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Top | DrawTextFormat.Left, textAlpha, Color.White, trackWindow);
                y += 0.30f;
            }

            D3DDriver.GetInstance().GetDevice().BeginScene();
            userInterface.DrawControl(mainWindow);
            D3DDriver.GetInstance().GetDevice().EndScene();
        }
    }
}
