using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerBase
    {
        private ITweakerSettings settings;
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

        public ITweakerSettings Settings
        {
            get { return settings; }
        }

        public BoxControl MainWindow
        {
            get { return mainWindow; }
        }

        public DemoTweakerBase(ITweakerSettings settings)
        {
            startTime = 0;
            timeScale = 10.0f;
            userInterface = new UserInterface();
            this.settings = settings;
        }

        public virtual void Initialize(IDemoRegistrator registrator, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory)
        {
            this.registrator = registrator;
            startTime = registrator.StartTime;

            UserInterface.Initialize(graphicsFactory, textureFactory);
        }

        protected void CreateBaseControls()
        {
            mainWindow = new BoxControl(new Vector4(0.05f, 0.05f, 0.90f, 0.90f), 0, Color.Black, null);

            titleWindow = new BoxControl(new Vector4(0, 0, 1, 0.05f),
                Settings.Alpha, Settings.TitleColor, mainWindow);
            int seconds = (int)Time.CurrentTime;
            int hundreds = (int)((Time.CurrentTime - seconds) * 100);
            string titleString = "DDXX Tweaker - " + seconds.ToString("D3") + "." + hundreds.ToString("D2");
            titleText = new TextControl(titleString, new Vector4(0, 0, 1, 1), TextFormatting.Center | TextFormatting.VerticalCenter,
                Settings.TextAlpha, Color.White, titleWindow);
        }

        protected BoxControl CreateTimeControls()
        {
            timeWindow = new BoxControl(new Vector4(0, 0.05f, 1, 0.95f),
                Settings.Alpha, Settings.TimeColor, mainWindow);
            BoxControl timelineWindow = new BoxControl(new Vector4(0.02f, 0.04f, 0.96f, 0.92f), 0, Color.Black, timeWindow);
            CreateTimeLine(timelineWindow);
            return timelineWindow;
        }

        private void CreateTimeLine(BoxControl timelineWindow)
        {
            new LineControl(new Vector4(0, 0.01f, 1, 0), Settings.Alpha, Color.White, timelineWindow);
            for (int i = 0; i < 5; i++)
            {
                float x = i / 4.0f;
                float t = StartTime + timeScale * i / 4.0f;
                new TextControl(t.ToString(), new Vector2(x, 0), TextFormatting.Bottom | TextFormatting.Center, Settings.Alpha, Color.White, timelineWindow);
                new LineControl(new Vector4(x, 0, 0, 0.02f), Settings.Alpha, Color.White, timelineWindow);
            }
            while ((Time.CurrentTime - StartTime) / timeScale > 0.9f)
                StartTime += timeScale * 0.9f;
            new LineControl(new Vector4((Time.CurrentTime - StartTime) / timeScale, 0, 0, 1), Settings.Alpha, Color.White, timelineWindow);
        }
    }
}
