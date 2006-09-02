using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.DemoFramework
{
    public class Track
    {
        List<IDemoEffect> effects = new List<IDemoEffect>();
        List<IDemoPostEffect> postEffects = new List<IDemoPostEffect>();

        private static float searchTime;
        private static bool IsWithinTime(IRegisterable registerable)
        {
            if (registerable.StartTime <= searchTime && registerable.EndTime >= searchTime)
                return true;
            return false;
        }

        private static int CompareRegisterableByTime(IRegisterable x, IRegisterable y)
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
            get { return effects.ToArray(); }
        }
        internal IDemoPostEffect[] PostEffects
        {
            get { return postEffects.ToArray(); }
        }

        internal void Register(IDemoEffect effect)
        {
            effects.Add(effect);
            effects.Sort(CompareRegisterableByTime);
        }
        internal void Register(IDemoPostEffect postEffect)
        {
            postEffects.Add(postEffect);
            postEffects.Sort(CompareRegisterableByTime);
        }


        internal IDemoEffect[] GetEffects(float time)
        {
            searchTime = time;
            List<IDemoEffect> valid = effects.FindAll(IsWithinTime);
            return valid.ToArray();
        }
        internal IDemoPostEffect[] GetPostEffects(float time)
        {
            searchTime = time;
            List<IDemoPostEffect> valid = postEffects.FindAll(IsWithinTime);
            return valid.ToArray();
        }

        internal bool IsActive(float p)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
