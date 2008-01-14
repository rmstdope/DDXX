using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TweakableDemo : TweakableObjectBase<IDemoRegistrator>
    {
        public TweakableDemo(IDemoRegistrator target)
            : base(target)
        {
        }

        public override int NumVisableVariables
        {
            get { return 3; }
        }

        protected override int NumSpecificVariables
        {
            get { return Target.Tracks.Count; }
        }

        protected override string SpecificVariableName(int index)
        {
            return "Track " + index;
        }

        protected override ITweakableObject GetSpecificVariable(int index)
        {
            return Target.Tracks[index];
        }

        protected override void CreateSpecificVariableControl(TweakerStatus status, int index, float y, ITweakerSettings settings)
        {
            float height = status.VariableSpacing * 0.9f;
            BoxControl trackWindow = new BoxControl(new Vector4(0, y, 1, height), settings.Alpha, GetBoxColor(status, index, settings), status.RootControl);
            new TextControl("Track #" + index, new Vector4(0, 0, 1, 1), TextFormatting.Top | TextFormatting.Left, settings.TextAlpha, GetTextColor(status, index, -1), trackWindow);
            CreateEffectControls(index, trackWindow, status.StartTime, status.TimeScale, settings);
        }

        private void CreateEffectControls(int track, BoxControl trackWindow, float startTime, float timeScale, ITweakerSettings settings)
        {
            IRegisterable[] effects = Target.Tracks[track].GetEffects(startTime, startTime + timeScale);
            IRegisterable[] postEffects = Target.Tracks[track].GetPostEffects(startTime, startTime + timeScale);
            IRegisterable[] allEffects = new IRegisterable[effects.Length + postEffects.Length];
            Array.Copy(effects, allEffects, effects.Length);
            Array.Copy(postEffects, 0, allEffects, effects.Length, postEffects.Length);
            float ey = 0.24f;
            foreach (IRegisterable effect in allEffects)
            {
                float ex1 = (effect.StartTime - startTime) / timeScale;
                if (ex1 < 0)
                    ex1 = 0;
                float ex2 = (effect.EndTime - startTime) / timeScale;
                if (ex2 > 1)
                    ex2 = 1;
                new TextControl(effect.GetType().Name, new Vector2(ex1, ey), TextFormatting.Bottom | TextFormatting.Left, settings.Alpha, Color.SkyBlue, trackWindow);
                new LineControl(new Vector4(ex1, ey, ex2 - ex1, 0), settings.Alpha, Color.SkyBlue, trackWindow);
                ey += 0.14f;
                if (ey > 1)
                    ey = 0.24f;
            }
        }

    }
}
