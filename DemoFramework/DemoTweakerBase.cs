using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerBase
    {
        private TweakerSettings settings;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private float startTime;
        private float timeScale;

        private BoxControl mainWindow;
        private BoxControl titleWindow;
        private TextControl titleText;
        protected BoxControl timeWindow;

        public IUserInterface UserInterface
        {
            set { userInterface = value; }
            get { return userInterface; }
        }

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public float TimeScale
        {
            get { return timeScale; }
            set { timeScale = value; }
        }

        public IDemoRegistrator Registrator
        {
            get { return registrator; }
        }

        public TweakerSettings Settings
        {
            get { return settings; }
        }

        public BoxControl MainWindow
        {
            get { return mainWindow; }
        }

        public DemoTweakerBase(TweakerSettings settings)
        {
            startTime = 0;
            timeScale = 10.0f;
            userInterface = new UserInterface();
            this.settings = settings;
        }

        public void Initialize(IDemoRegistrator registrator)
        {
            this.registrator = registrator;
            startTime = registrator.StartTime;

            UserInterface.Initialize();
        }

        protected void CreateBaseControls()
        {
            mainWindow = new BoxControl(new RectangleF(0.05f, 0.05f, 0.90f, 0.90f), 0.0f, Color.Black, null);

            titleWindow = new BoxControl(new RectangleF(0.0f, 0.0f, 1.0f, 0.05f),
                Settings.Alpha, Settings.TitleColor, mainWindow);
            titleText = new TextControl("DDXX Tweaker", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f),
                DrawTextFormat.Center | DrawTextFormat.VerticalCenter,
                Settings.TextAlpha, Color.White, titleWindow);
        }

        protected BoxControl CreateTimeControls()
        {
            timeWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f),
                Settings.Alpha, Settings.TimeColor, mainWindow);
            BoxControl timelineWindow = new BoxControl(new RectangleF(0.02f, 0.04f, 0.96f, 0.92f), 0.0f, Color.Black, timeWindow);
            CreateTimeLine(timelineWindow);
            return timelineWindow;
        }

        private void CreateTimeLine(BoxControl timelineWindow)
        {
            new LineControl(new RectangleF(0.0f, 0.01f, 1.0f, 0.0f), Settings.Alpha, Color.White, timelineWindow);
            for (int i = 0; i < 5; i++)
            {
                float x = i / 4.0f;
                float t = StartTime + timeScale * i / 4.0f;
                new TextControl(t.ToString(), new PointF(x, 0.0f), DrawTextFormat.Bottom | DrawTextFormat.Center, Settings.Alpha, Color.White, timelineWindow);
                new LineControl(new RectangleF(x, 0.0f, 0.0f, 0.02f), Settings.Alpha, Color.White, timelineWindow);
            }
            while ((Time.StepTime - StartTime) / timeScale > 0.9f)
                StartTime += timeScale * 0.9f;
            new LineControl(new RectangleF((Time.StepTime - StartTime) / timeScale, 0.0f, 0.0f, 1.0f), Settings.Alpha, Color.White, timelineWindow);
        }
    }
}
