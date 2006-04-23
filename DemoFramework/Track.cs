using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFramework
{
    public class Track
    {
        List<IEffect> effects = new List<IEffect>();

        private static float searchTime;
        private static bool IsWithinTime(IEffect effect)
        {
            if (effect.StartTime <= searchTime && effect.EndTime >= searchTime)
                return true;
            return false;
        }

        private static int CompareEffectsByTime(IEffect x, IEffect y)
        {
            if (x.StartTime < y.StartTime)
                return -1;
            else if (y.StartTime < x.StartTime)
                return 1;
            else if (x.EndTime < y.EndTime)
                return -1;
            else if (y.EndTime < x.EndTime)
                return 1;
            return 0;
        }

        internal IEffect[] Effects
        {
            get
            {
                return effects.ToArray();
            }
        }

        internal void Register(IEffect effect)
        {
            effects.Add(effect);
            effects.Sort(CompareEffectsByTime);
        }

        internal IEffect[] GetEffects(float time)
        {
            searchTime = time;
            List<IEffect> valid = effects.FindAll(IsWithinTime);
            return valid.ToArray();
        }
    }
}
