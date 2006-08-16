using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public class Track
    {
        List<IDemoEffect> effects = new List<IDemoEffect>();

        private static float searchTime;
        private static bool IsWithinTime(IDemoEffect effect)
        {
            if (effect.StartTime <= searchTime && effect.EndTime >= searchTime)
                return true;
            return false;
        }

        private static int CompareEffectsByTime(IDemoEffect x, IDemoEffect y)
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

        internal IDemoEffect[] Effects
        {
            get
            {
                return effects.ToArray();
            }
        }

        internal void Register(IDemoEffect effect)
        {
            effects.Add(effect);
            effects.Sort(CompareEffectsByTime);
        }

        internal IDemoEffect[] GetEffects(float time)
        {
            searchTime = time;
            List<IDemoEffect> valid = effects.FindAll(IsWithinTime);
            return valid.ToArray();
        }

        internal bool IsActive(float p)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
