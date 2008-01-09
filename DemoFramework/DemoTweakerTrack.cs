using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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

        public bool Exiting
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

        public IDemoTweaker HandleInput(IInputDriver inputDriver)
        {
            if (inputDriver.UpPressedNoRepeat())
            {
                KeyUp();
            }
            if (inputDriver.DownPressedNoRepeat())
            {
                KeyDown();
            }
            return null;
        }

        public void Draw()
        {
            //CreateBaseControls();
            //BoxControl control = CreateTimeControls();
            //CreateEffects(control);

            UserInterface.DrawControl(MainWindow);
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
                if (ex1 < 0)
                    ex1 = 0;
                float ex2 = (allEffects[i].EndTime - StartTime) / timeScale;
                if (ex2 > 1)
                    ex2 = 1;
                if (ex1 < 1 && ex2 > 0)
                {
                    BoxControl trackWindow = new BoxControl(new Vector4(ex1, y, ex2 - ex1, 0.05f), Settings.Alpha, boxColor, timelineWindow);
                    new TextControl(allEffects[i].GetType().Name, new Vector4(0, 0, 1, 1), TextFormatting.Center | TextFormatting.VerticalCenter, Settings.TextAlpha, textColor, trackWindow);
                }
                else if (ex1 >= 1.0f)
                {
                    BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, 0.5f), 0, boxColor, timelineWindow);
                    new TextControl(allEffects[i].GetType().Name + "-->", new Vector4(0, 0, 1, 1), TextFormatting.Right | TextFormatting.VerticalCenter, Settings.TextAlpha, textColor, trackWindow);
                }
                else
                {
                    BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, 0.05f), 0, boxColor, timelineWindow);
                    new TextControl("<--" + allEffects[i].GetType().Name, new Vector4(0, 0, 1, 1), TextFormatting.Left | TextFormatting.VerticalCenter, Settings.TextAlpha, textColor, trackWindow);
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

        public bool ShouldSave()
        {
            return true;
        }

    }
}
