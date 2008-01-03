using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class DemoTweakerDemo : DemoTweakerBase, IDemoTweaker
    {
        private int currentTrack;

        public object IdentifierToChild() { return Registrator.Tracks[currentTrack]; }
        public void IdentifierFromParent(object id) { }

        public bool Quit
        {
            get { return false; }
        }

        public bool Exiting
        {
            get { return false; }
        }

        public int CurrentTrack
        {
            get { return currentTrack; }
        }

        public DemoTweakerDemo(ITweakerSettings settings)
            : base(settings)
        {
            currentTrack = 0;
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
            if (currentTrack == Registrator.Tracks.Count)
                currentTrack--;
        }

        public bool HandleInput(IInputDriver inputDriver)
        {
            bool handled = false;

            if (inputDriver.UpPressedNoRepeat())
            {
                KeyUp();
                handled = true;
            }
            if (inputDriver.DownPressedNoRepeat())
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
            CreateTracks(control);

            UserInterface.DrawControl(MainWindow);
        }

        private void CreateTracks(BoxControl timelineWindow)
        {
            Color color;
            float y = 0.05f;
            int startTrack = GetStartTrack();
            for (int i = startTrack; i < startTrack + 3; i++)
            {
                if (i >= Registrator.Tracks.Count)
                    continue;
                if (i == currentTrack)
                    color = Settings.SelectedColor;
                else
                    color = Settings.UnselectedColor;
                BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, 0.25f), Settings.Alpha, color, timelineWindow);
                new TextControl("Track " + i, new Vector4(0, 0, 1, 1), TextFormatting.Top | TextFormatting.Left, Settings.TextAlpha, Color.White, trackWindow);
                y += 0.35f;
                DrawEffectsInTrack(i, trackWindow);

            }
        }

        private int GetStartTrack()
        {
            int startTrack = currentTrack - 1;
            if (currentTrack == Registrator.Tracks.Count - 1)
                startTrack--;
            if (startTrack < 0)
                startTrack = 0;
            return startTrack;
        }

        private void DrawEffectsInTrack(int track, BoxControl trackWindow)
        {
            IRegisterable[] effects = Registrator.Tracks[track].GetEffects(StartTime, StartTime + TimeScale);
            IRegisterable[] postEffects = Registrator.Tracks[track].GetPostEffects(StartTime, StartTime + TimeScale);
            IRegisterable[] allEffects = new IRegisterable[effects.Length + postEffects.Length];
            Array.Copy(effects, allEffects, effects.Length);
            Array.Copy(postEffects, 0, allEffects, effects.Length, postEffects.Length);
            float ey = 0.24f;
            foreach (IRegisterable effect in allEffects)
            {
                float ex1 = (effect.StartTime - StartTime) / TimeScale;
                if (ex1 < 0)
                    ex1 = 0;
                float ex2 = (effect.EndTime - StartTime) / TimeScale;
                if (ex2 > 1)
                    ex2 = 1;
                new TextControl(effect.GetType().Name, new Vector2(ex1, ey), TextFormatting.Bottom | TextFormatting.Left, Settings.Alpha, Color.SkyBlue, trackWindow);
                new LineControl(new Vector4(ex1, ey, ex2 - ex1, 0), Settings.Alpha, Color.SkyBlue, trackWindow);
                ey += 0.14f;
                if (ey > 1)
                    ey = 0.24f;
            }
        }

        public bool ShouldSave()
        {
            return true;
        }

    }
}
