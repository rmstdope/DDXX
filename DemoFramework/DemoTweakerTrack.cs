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
    public class DemoTweakerTrack : DemoTweakerBase, IDemoTweaker
    {
        private int currentEffect;
        private Track currentTrack;
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

        public DemoTweakerTrack(ITweakerSettings settings)
            : base(settings)
        {
            currentEffect = 0;
            timeScale = 10.0f;
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
            CreateBaseControls();
            BoxControl control = CreateTimeControls();
            CreateEffects(control);

            D3DDriver.GetInstance().Device.BeginScene();
            UserInterface.DrawControl(MainWindow);
            D3DDriver.GetInstance().Device.EndScene();
        }

        private void CreateEffects(BoxControl timelineWindow)
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
                float ex1 = (allEffects[i].StartTime - StartTime) / timeScale;
                if (ex1 < 0.0f)
                    ex1 = 0.0f;
                float ex2 = (allEffects[i].EndTime - StartTime) / timeScale;
                if (ex2 > 1.0f)
                    ex2 = 1.0f;
                if (ex1 < 1.0f && ex2 > 0.0f)
                {
                    BoxControl trackWindow = new BoxControl(new RectangleF(ex1, y, ex2 - ex1, 0.05f), Settings.Alpha, boxColor, timelineWindow);
                    new TextControl(allEffects[i].GetType().Name, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Settings.TextAlpha, textColor, trackWindow);
                }
                else if (ex1 >= 1.0f)
                {
                    BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.05f), 0.0f, boxColor, timelineWindow);
                    new TextControl(allEffects[i].GetType().Name + "-->", new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Right | DrawTextFormat.VerticalCenter, Settings.TextAlpha, textColor, trackWindow);
                }
                else
                {
                    BoxControl trackWindow = new BoxControl(new RectangleF(0.0f, y, 1.0f, 0.05f), 0.0f, boxColor, timelineWindow);
                    new TextControl("<--" + allEffects[i].GetType().Name, new RectangleF(0.0f, 0.0f, 1.0f, 1.0f), DrawTextFormat.Left | DrawTextFormat.VerticalCenter, Settings.TextAlpha, textColor, trackWindow);
                }
                y += 0.075f;
            }
        }

        private void GetColors(int i, out Color boxColor, out Color textColor)
        {
            if (i == currentEffect)
            {
                boxColor = Settings.SelectedColor;
                textColor = Color.White;
            }
            else
            {
                boxColor = Settings.UnselectedColor;
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

        private IRegisterable[] GetEffectsAndPostEffects(Track track)
        {
            IRegisterable[] effects = track.Effects;
            IRegisterable[] postEffects = track.PostEffects;
            IRegisterable[] allEffects = new IRegisterable[effects.Length + postEffects.Length];
            Array.Copy(effects, allEffects, effects.Length);
            Array.Copy(postEffects, 0, allEffects, effects.Length, postEffects.Length);
            return allEffects;
        }

        public bool ShouldSave(IInputDriver inputDriver)
        {
            return true;
        }

    }
}
