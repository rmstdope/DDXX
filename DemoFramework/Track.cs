using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class Track : ITrack
    {
        private PostProcessor postProcessor;
        private SpriteBatch spriteBatch;

        List<IDemoEffect> effects = new List<IDemoEffect>();
        List<IDemoPostEffect> postEffects = new List<IDemoPostEffect>();

        private static float searchTime;
        private static bool IsWithinTime(IRegisterable registerable)
        {
            if (registerable.StartTime <= searchTime && registerable.EndTime >= searchTime)
                return true;
            return false;
        }

        public static int CompareRegisterableByTime(IRegisterable x, IRegisterable y)
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
        public List<IDemoEffect> EffectList
        {
            get { return effects; }
        }
        public List<IDemoPostEffect> PostEffectList
        {
            get { return postEffects; }
        }

        public Track()
        {
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

        public float StartTime
        {
            get
            {
                float minTime = float.MaxValue;
                foreach (IDemoEffect effect in effects)
                {
                    if (effect.StartTime < minTime)
                        minTime = effect.StartTime;
                }
                foreach (IDemoPostEffect effect in postEffects)
                {
                    if (effect.StartTime < minTime)
                        minTime = effect.StartTime;
                }
                if (minTime == float.MaxValue)
                    minTime = 0;
                return minTime;
            }
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

        public void Initialize(IGraphicsFactory graphicsFactory, IDemoMixer mixer, PostProcessor postProcessor)
        {
            this.spriteBatch = new SpriteBatch(graphicsFactory.GraphicsDevice);
            this.postProcessor = postProcessor;
            foreach (IDemoEffect effect in effects)
                effect.Initialize(graphicsFactory, mixer, postProcessor);
            foreach (IDemoPostEffect effect in postEffects)
                effect.Initialize(graphicsFactory, postProcessor);
        }

        public void Step()
        {
            foreach (IDemoEffect effect in GetEffects(Time.CurrentTime))
                if (IsWithinTime(effect))
                    effect.Step();
        }

        public RenderTarget2D Render(GraphicsDevice device, RenderTarget2D renderTarget, RenderTarget2D renderTargetNoMultiSampling, Color backgroundColor)
        {
            device.SetRenderTarget(renderTarget);
            //device.DepthStencilState.DepthBufferEnable = true;

            //if (D3DDriver.GetInstance().Description.useStencil)
            //    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, backgroundColor, 1.0f, 0);
            //else
            device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backgroundColor, 1.0f, 0);

            RenderEffects(device);
            device.SetRenderTarget(null);//.ResolveRenderTarget(0);
            //device.DepthStencilState.DepthBufferEnable = false;
            if (renderTargetNoMultiSampling != renderTarget)
            {
                device.SetRenderTarget(renderTargetNoMultiSampling);
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);
                spriteBatch.End();
                device.SetRenderTarget(null);//.ResolveRenderTarget(0);
            }

            postProcessor.StartFrame(renderTargetNoMultiSampling);
            RenderPostEffects();

            return postProcessor.OutputTexture;
        }

        private void RenderEffects(GraphicsDevice device)
        {
            //device.BeginScene();
            IDemoEffect[] activeEffects = GetEffects(Time.CurrentTime);
            int minOrder = 1000;
            int maxOrder = -1000;
            foreach (IDemoEffect effect in activeEffects)
            {
                if (effect.DrawOrder > maxOrder)
                    maxOrder = effect.DrawOrder;
                if (effect.DrawOrder < minOrder)
                    minOrder = effect.DrawOrder;
            }
            for (int i = minOrder; i <= maxOrder; i++)
                foreach (IDemoEffect effect in activeEffects)
                    if (effect.DrawOrder == i)
                        effect.Render();
            //device.EndScene();
        }

        private void RenderPostEffects()
        {
            IDemoPostEffect[] activePostEffects = GetPostEffects(Time.CurrentTime);
            int minOrder = 1000;
            int maxOrder = -1000;
            foreach (IDemoPostEffect effect in activePostEffects)
            {
                if (effect.DrawOrder > maxOrder)
                    maxOrder = effect.DrawOrder;
                if (effect.DrawOrder < minOrder)
                    minOrder = effect.DrawOrder;
            }
            for (int i = minOrder; i <= maxOrder; i++)
                foreach (IDemoPostEffect effect in activePostEffects)
                    if (effect.DrawOrder == i)
                        effect.Render();
        }

        public void UpdateListener(EffectChangeListener effectChangeListener)
        {
            //foreach (IDemoEffect effect in effects)
            //    (effect as TweakableContainer).UpdateListener(effectChangeListener);
            //foreach (IDemoPostEffect effect in postEffects)
            //    (effect as TweakableContainer).UpdateListener(effectChangeListener);
        }

        public bool IsEffectRegistered(string name, Type type)
        {
            foreach (IDemoEffect effect in effects)
            {
                if (effect.Name == name && effect.GetType() == type)
                    return true;
            }
            return false;
        }

        public bool IsPostEffectRegistered(string name, Type type)
        {
            foreach (IDemoPostEffect effect in postEffects)
            {
                if (effect.Name == name && effect.GetType() == type)
                    return true;
            }
            return false;
        }

    }
}
