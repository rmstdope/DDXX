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

        public DemoTweakerDemo()
        {
            userInterface = new UserInterface();
            currentTrack = 0;
            startTime = 0;
            timeScale = 10.0f;
        }

        public void KeyUp()
        {
            currentTrack--;
            if (currentTrack == -1)
                currentTrack++;
        }

        public void KeyDown()
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

            CreateControls();
        }

        private void CreateControls()
        {
            mainWindow = new BoxControl(new RectangleF(0.05f, 0.05f, 0.90f, 0.90f), 0.0f, Color.Black, null);

            titleWindow = new BoxControl(new RectangleF(0.0f, 0.0f, 1.0f, 0.05f), alpha, titleColor, mainWindow);
            titleText = new TextControl("DDXX Tweaker", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, textAlpha, Color.White, titleWindow);

            timeWindow = new BoxControl(new RectangleF(0.0f, 0.05f, 1.0f, 0.95f), alpha, timeColor, mainWindow);
        }

        public void HandleInput(IInputDriver inputDriver)
        {
            if (inputDriver.KeyPressedNoRepeat(Key.UpArrow))
                KeyUp();
            if (inputDriver.KeyPressedNoRepeat(Key.DownArrow))
                KeyDown();
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
            timeWindow.Children.Clear();

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
                    color = selectedTrackColor;
                else
                    color = trackColor;
                BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.25f), alpha, color, timelineWindow);
                new TextControl("Track " + i, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Top | DrawTextFormat.Left, textAlpha, Color.White, trackWindow);
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
                new TextControl(effect.GetType().Name, new PointF(ex1, ey), DrawTextFormat.Bottom | DrawTextFormat.Left, alpha, Color.SkyBlue, trackWindow);
                new LineControl(new RectangleF(ex1, ey, ex2 - ex1, 0.0f), alpha, Color.SkyBlue, trackWindow);
                ey += 0.14f;
                if (ey > 1.0f)
                    ey = 0.2f;
            }
        }

        private void DrawTimeLine(BoxControl timelineWindow)
        {
            new LineControl(new RectangleF(0.0f, 0.01f, 1.0f, 0.0f), alpha, Color.White, timelineWindow);
            for (int i = 0; i < 5; i++)
            {
                float x = i / 4.0f;
                float t = startTime + timeScale * i / 4.0f;
                new TextControl(t.ToString(), new PointF(x, 0.0f), DrawTextFormat.Bottom | DrawTextFormat.Center, alpha, Color.White, timelineWindow);
                new LineControl(new RectangleF(x, 0.0f, 0.0f, 0.02f), alpha, Color.White, timelineWindow);
            }
            while ((Time.StepTime - startTime) / timeScale > 0.9f)
                startTime += timeScale * 0.9f;
            new LineControl(new RectangleF((Time.StepTime - startTime) / timeScale, 0.0f, 0.0f, 1.0f), alpha, Color.White, timelineWindow);
        }
    }
}
