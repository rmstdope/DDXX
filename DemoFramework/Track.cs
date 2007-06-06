using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public class Track : ITrack
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

        public IDemoEffect[] Effects
        {
            get { return effects.ToArray(); }
        }
        public IDemoPostEffect[] PostEffects
        {
            get { return postEffects.ToArray(); }
        }

        public void Register(IDemoEffect effect)
        {
            effects.Add(effect);
            effects.Sort(CompareRegisterableByTime);
        }
        public void Register(IDemoPostEffect postEffect)
        {
            postEffects.Add(postEffect);
            postEffects.Sort(CompareRegisterableByTime);
        }


        public IDemoEffect[] GetEffects(float time)
        {
            searchTime = time;
            List<IDemoEffect> valid = effects.FindAll(IsWithinTime);
            return valid.ToArray();
        }

        public IDemoEffect[] GetEffects(float startTime, float endTime)
        {
            List<IDemoEffect> valid = effects.FindAll(
                delegate(IDemoEffect effect) 
                {
                    if (effect.StartTime <= endTime && effect.EndTime >= startTime)
                        return true;
                    return false;
                });
            return valid.ToArray();
        }

        public IDemoPostEffect[] GetPostEffects(float time)
        {
            searchTime = time;
            List<IDemoPostEffect> valid = postEffects.FindAll(IsWithinTime);
            return valid.ToArray();
        }

        public IDemoPostEffect[] GetPostEffects(float startTime, float endTime)
        {
            List<IDemoPostEffect> valid = postEffects.FindAll(
                delegate(IDemoPostEffect effect)
                {
                    if (effect.StartTime <= endTime && effect.EndTime >= startTime)
                        return true;
                    return false;
                });
            return valid.ToArray();
        }

        public bool IsActive(float p)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float EndTime
        {
            get 
            {
                float maxTime = 0;
                foreach (IDemoEffect effect in effects)
                {
                    if (effect.EndTime > maxTime)
                        maxTime = effect.EndTime;
                }
                foreach (IDemoPostEffect effect in postEffects)
                {
                    if (effect.EndTime > maxTime)
                        maxTime = effect.EndTime;
                }
                return maxTime; 
            }
        }

        public void Initialize(IGraphicsFactory graphicsFactory, IDevice device, ITextureFactory textureFactory, 
            ITextureBuilder textureBuilder, IPostProcessor postProcessor)
        {
            foreach (IDemoEffect effect in effects)
                effect.Initialize(graphicsFactory, device);
            foreach (IDemoPostEffect effect in postEffects)
                effect.Initialize(postProcessor, textureFactory, textureBuilder, device);
        }

        public void Step()
        {
            foreach (IDemoEffect effect in GetEffects(Time.StepTime))
                if (IsWithinTime(effect))
                    effect.Step();
        }

        public void Render(IDevice device)
        {
            device.BeginScene();
            IDemoEffect[] activeEffects = GetEffects(Time.StepTime);
            foreach (IDemoEffect effect in activeEffects)
                effect.Render();
            device.EndScene();
            IDemoPostEffect[] activePostEffects = GetPostEffects(Time.StepTime);
            foreach (IDemoPostEffect effect in activePostEffects)
                effect.Render();
        }

        public void UpdateListener(IEffectChangeListener effectChangeListener)
        {
            foreach (IDemoEffect effect in effects)
                effect.UpdateListener(effectChangeListener);
            foreach (IDemoPostEffect effect in postEffects)
                effect.UpdateListener(effectChangeListener);
        }
    }
}
