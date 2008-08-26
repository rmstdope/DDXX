using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectAdapter : IEffect
    {
        private Effect effect;

        public EffectAdapter(Effect effect)
        {
            this.effect = effect;
        }

        public Effect DxEffect { get { return effect; } }

        #region IEffect Members

        public string Creator
        {
            get { return effect.Creator; }
        }

        public IEffectTechnique CurrentTechnique
        {
            get
            {
                return new EffectTechniqueAdapter(effect.CurrentTechnique);
            }
            set
            {
                effect.CurrentTechnique = (value as EffectTechniqueAdapter).DxEffectTechnique;
            }
        }

        public EffectPool EffectPool
        {
            get { return effect.EffectPool; }
        }

        public ICollectionAdapter<IEffectFunction> Functions
        {
            get { return new EffectFunctionCollectionAdapter(effect.Functions); }
        }

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(effect.GraphicsDevice); }
        }

        public ICollectionAdapter<IEffectParameter> Parameters
        {
            get { return new EffectParameterCollectionAdapter(effect.Parameters); }
        }

        public ICollectionAdapter<IEffectTechnique> Techniques
        {
            get { return new EffectTechniqueCollectionAdapter(effect.Techniques); }
        }

        public void Begin()
        {
            effect.Begin();
        }

        public void Begin(SaveStateMode saveStateMode)
        {
            effect.Begin(saveStateMode);
        }

        public virtual IEffect Clone(IGraphicsDevice device)
        {
            return new EffectAdapter(effect.Clone((device as GraphicsDeviceAdapter).DxGraphicsDevice));
        }

        public void CommitChanges()
        {
            effect.CommitChanges();
        }

#if (!XBOX)
        public string Disassemble(bool enableColorCode)
        {
            return effect.Disassemble(enableColorCode);
        }
#endif

        public void End()
        {
            effect.End();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            effect.Dispose();
        }

        #endregion
    }
}
