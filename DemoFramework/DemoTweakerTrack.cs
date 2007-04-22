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
    public class DemoTweakerTrack : IDemoTweaker
    {
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private TweakerSettings tweakerSettings;

        private BoxControl mainWindow;
        private BoxControl titleWindow;
        private TextControl titleText;
        private BoxControl timeWindow;

        private int currentEffect;
        private Track currentTrack;
        private float startTime;
        private float timeScale;

        private const int NumVisableEffects = 13;

        public object IdentifierToChild() 
        { 
            if (currentEffect >= currentTrack.Effects.Length)
                return currentTrack.PostEffects[currentEffect - currentTrack.Effects.Length];
            else
                return currentTrack.Effects[currentEffect];
        }
        public void IdentifierFromParent(object id) 
        { 
            if (id.GetType() != typeof(Track))
                throw new DDXXException("Incorrect tweaker type received from parent.");
            CurrentTrack = (Track)id; 
        }

        public bool Quit
        {
            get { return false; }
        }

        public Track CurrentTrack
        {
            set 
            {
                if (value != currentTrack)
                    currentEffect = 0;
                currentTrack = value; 
            }
        }

        public int CurrentEffect
        {
            get { return currentEffect; }
        }

        public IUserInterface UserInterface
        {
            set { userInterface = value; }
        }

        public DemoTweakerTrack(TweakerSettings settings)
        {
            userInterface = new UserInterface();
            currentEffect = 0;
            startTime = 0;
            timeScale = 10.0f;
            tweakerSettings = settings;
        }

        public void KeyUp()
        {
            currentEffect--;
            if (currentEffect == -1)
                currentEffect++;
        }

        public void KeyDown()
        {
            currentEffect++;
            if (currentEffect == currentTrack.Effects.Length + currentTrack.PostEffects.Length)
                currentEffect--;
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

            DrawEffects(timelineWindow);
        }

        private void DrawEffects(BoxControl timelineWindow)
        {
            IRegisterable[] allEffects = GetEffectsAndPostEffects(currentTrack);
            Color boxColor;
            Color textColor;
            float y = 0.05f;
            int startEffect = GetStartEffect(allEffects.Length);
            for (int i = startEffect; i < startEffect + NumVisableEffects; i++)
            {
                if (i >= allEffects.Length)
                    continue;
                GetColors(i, out boxColor, out textColor);
                float ex1 = (allEffects[i].StartTime - startTime) / timeScale;
                if (ex1 < 0.0f)
                    ex1 = 0.0f;
                float ex2 = (allEffects[i].EndTime - startTime) / timeScale;
                if (ex2 > 1.0f)
                    ex2 = 1.0f;
                if (ex1 < 1.0f && ex2 > 0.0f)
                {
                    BoxControl trackWindow = new BoxControl(new RectangleF(ex1, y, ex2 - ex1, 0.05f), tweakerSettings.Alpha, boxColor, timelineWindow);
                    new TextControl(allEffects[i].GetType().Name, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, tweakerSettings.TextAlpha, textColor, trackWindow);
                }
                else if (ex1 >= 1.0f)
                {
                    BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.05f), 0.0f, boxColor, timelineWindow);
                    new TextControl(allEffects[i].GetType().Name + "-->", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Right | DrawTextFormat.VerticalCenter, tweakerSettings.TextAlpha, textColor, trackWindow);
                }
                else
                {
                    BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.05f), 0.0f, boxColor, timelineWindow);
                    new TextControl("<--" + allEffects[i].GetType().Name, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Left | DrawTextFormat.VerticalCenter, tweakerSettings.TextAlpha, textColor, trackWindow);
                }
                y += 0.075f;
            }
        }

        private void GetColors(int i, out Color boxColor, out Color textColor)
        {
            if (i == currentEffect)
            {
                boxColor = tweakerSettings.SelectedColor;
                textColor = Color.White;
            }
            else
            {
                boxColor = tweakerSettings.UnselectedColor;
                textColor = Color.Gray;
            }
        }

        private int GetStartEffect(int numEffects)
        {
            int startEffect = currentEffect - (NumVisableEffects / 2);
            while (startEffect > 0 && startEffect + NumVisableEffects > numEffects)
                startEffect--;
            if (startEffect < 0)
                startEffect = 0;
            return startEffect;
        }

        //private void DrawEffectsInTrack(int track, BoxControl trackWindow)
        //{
        //    IRegisterable[] allEffects = GetEffectsAndPostEffects(track);
        //    float ey = 0.24f;
        //    foreach (IRegisterable effect in allEffects)
        //    {
        //        float ex1 = (effect.StartTime - startTime) / timeScale;
        //        if (ex1 < 0.0f)
        //            ex1 = 0.0f;
        //        float ex2 = (effect.EndTime - startTime) / timeScale;
        //        if (ex2 > 1.0f)
        //            ex2 = 1.0f;
        //        new TextControl(effect.GetTweakableType().Name, new PointF(ex1, ey), DrawTextFormat.Bottom | DrawTextFormat.Left, tweakerSettings.Alpha, Color.SkyBlue, trackWindow);
        //        new LineControl(new RectangleF(ex1, ey, ex2 - ex1, 0.0f), tweakerSettings.Alpha, Color.SkyBlue, trackWindow);
        //        ey += 0.14f;
        //        if (ey > 1.0f)
        //            ey = 0.2f;
        //    }
        //}

        private IRegisterable[] GetEffectsAndPostEffects(Track track)
        {
            IRegisterable[] effects = track.Effects; //GetEffects(startTime, startTime + timeScale);
            IRegisterable[] postEffects = track.PostEffects; //.GetPostEffects(startTime, startTime + timeScale);
            IRegisterable[] allEffects = new IRegisterable[effects.Length + postEffects.Length];
            Array.Copy(effects, allEffects, effects.Length);
            Array.Copy(postEffects, 0, allEffects, effects.Length, postEffects.Length);
            return allEffects;
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
