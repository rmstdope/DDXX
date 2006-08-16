using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class EffectContainer
    {
        private IEffect effect;
        private EffectHandle technique;

        public IEffect Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public EffectHandle Technique
        {
            get { return technique; }
            set { technique = value; }
        }

        public EffectContainer(IEffect effect, EffectHandle technique)
        {
            this.effect = effect;
            this.technique = technique;
        }
    }
}
