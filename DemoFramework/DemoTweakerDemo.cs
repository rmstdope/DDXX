using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Input;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerDemo : IDemoTweaker
    {
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private TweakerSettings tweakerSettings;

        private BoxControl mainWindow;
        private BoxControl titleWindow;
        private TextControl titleText;
        private BoxControl timeWindow;

        private int currentTrack;
        private float startTime;
        private float timeScale;

        public object IdentifierToChild() { return registrator.Tracks[currentTrack]; }
        public void IdentifierFromParent(object id) { }

        public bool Quit
        {
            get { return false; }
        }

        public int CurrentTrack
        {
            get { return currentTrack; }
        }

        public IUserInterface UserInterface
        {
            set { userInterface = value; }
        }

        public DemoTweakerDemo(TweakerSettings settings)
        {
            userInterface = new UserInterface();
            currentTrack = 0;
            startTime = 0;
            timeScale = 10.0f;
            tweakerSettings = settings;
        }

        private void KeyUp()
        {
            currentTrack--;
            if (currentTrack == -1)
                currentTrack++;
        }

        private void KeyDown()
        {
            currentTrack++;
            if (currentTrack == registrator.Tracks.Count)
                currentTrack--;
        }

        public void Initialize(IDemoRegistrator registrator)
        {
            this.registrator = registrator;
            startTime = registrator.StartTime;

            userInterface.Initialize();
        }

        private void CreateBaseControls()
        {
            mainWindow = new BoxControl(new RectangleF(0.05f, 0.05f, 0.90f, 0.90f), 0.0f, Color.Black, null);

            titleWindow = new BoxControl(new RectangleF(0.0f, 0.0f, 1.0f, 0.05f),
                tweakerSettings.Alpha, tweakerSettings.TitleColor, mainWindow);
            titleText = new TextControl("DDXX Tweaker", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), 
                DrawTextFormat.Center | DrawTextFormat.VerticalCenter, 
                tweakerSettings.TextAlpha, Color.White, titleWindow);

            timeWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f), 
                tweakerSettings.Alpha, tweakerSettings.TimeColor, mainWindow);
        }

        public bool HandleInput(IInputDriver inputDriver)
        {
            bool handled = false;

            if (inputDriver.KeyPressedNoRepeat(Key.UpArrow))
            {
                KeyUp();
                handled = true;
            }
            if (inputDriver.KeyPressedNoRepeat(Key.DownArrow))
            {
                KeyDown();
                handled = true;
            }

            return handled;
        }

        public void Draw()
        {
            DrawTimeWindow();

            D3DDriver.GetInstance().Device.BeginScene();
            userInterface.DrawControl(mainWindow);
            D3DDriver.GetInstance().Device.EndScene();
        }

        private void DrawTimeWindow()
        {
            CreateBaseControls();

            BoxControl timelineWindow = new BoxControl(new RectangleF(0.02f, 0.04f, 0.96f, 0.92f), 0.0f, Color.Black, timeWindow);

            DrawTimeLine(timelineWindow);

            DrawTracks(timelineWindow);
        }

        private void DrawTracks(BoxControl timelineWindow)
        {
            Color color;
            float y = 0.05f;
            int startTrack = GetStartTrack();
            for (int i = startTrack; i < startTrack + 3; i++)
            {
                if (i >= registrator.Tracks.Count)
                    continue;
                if (i == currentTrack)
                    color = tweakerSettings.SelectedColor;
                else
                    color = tweakerSettings.UnselectedColor;
                BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.25f), tweakerSettings.Alpha, color, timelineWindow);
                new TextControl("Track " + i, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Top | DrawTextFormat.Left, tweakerSettings.TextAlpha, Color.White, trackWindow);
                y += 0.35f;
                DrawEffectsInTrack(i, trackWindow);

            }
        }

        private int GetStartTrack()
        {
            int startTrack = currentTrack - 1;
            if (currentTrack == registrator.Tracks.Count - 1)
                startTrack--;
            if (startTrack < 0)
                startTrack = 0;
            return startTrack;
        }

        private void DrawEffectsInTrack(int track, BoxControl trackWindow)
        {
            IRegisterable[] effects = registrator.Tracks[track].GetEffects(startTime, startTime + timeScale);
            IRegisterable[] postEffects = registrator.Tracks[track].GetPostEffects(startTime, startTime + timeScale);
            IRegisterable[] allEffects = new IRegisterable[effects.Length + postEffects.Length];
            Array.Copy(effects, allEffects, effects.Length);
            Array.Copy(postEffects, 0, allEffects, effects.Length, postEffects.Length);
            float ey = 0.24f;
            foreach (IRegisterable effect in allEffects)
            {
                float ex1 = (effect.StartTime - startTime) / timeScale;
                if (ex1 < 0.0f)
                    ex1 = 0.0f;
                float ex2 = (effect.EndTime - startTime) / timeScale;
                if (ex2 > 1.0f)
                    ex2 = 1.0f;
                new TextControl(effect.GetType().Name, new PointF(ex1, ey), DrawTextFormat.Bottom | DrawTextFormat.Left, tweakerSettings.Alpha, Color.SkyBlue, trackWindow);
                new LineControl(new RectangleF(ex1, ey, ex2 - ex1, 0.0f), tweakerSettings.Alpha, Color.SkyBlue, trackWindow);
                ey += 0.14f;
                if (ey > 1.0f)
                    ey = 0.2f;
            }
        }

        private void DrawTimeLine(BoxControl timelineWindow)
        {
            new LineControl(new RectangleF(0.0f, 0.01f, 1.0f, 0.0f), tweakerSettings.Alpha, Color.White, timelineWindow);
            for (int i = 0; i < 5; i++)
            {
                float x = i / 4.0f;
                float t = startTime + timeScale * i / 4.0f;
                new TextControl(t.ToString(), new PointF(x, 0.0f), DrawTextFormat.Bottom | DrawTextFormat.Center, tweakerSettings.Alpha, Color.White, timelineWindow);
                new LineControl(new RectangleF(x, 0.0f, 0.0f, 0.02f), tweakerSettings.Alpha, Color.White, timelineWindow);
            }
            while ((Time.StepTime - startTime) / timeScale > 0.9f)
                startTime += timeScale * 0.9f;
            new LineControl(new RectangleF((Time.StepTime - startTime) / timeScale, 0.0f, 0.0f, 1.0f), tweakerSettings.Alpha, Color.White, timelineWindow);
        }
    }
}
