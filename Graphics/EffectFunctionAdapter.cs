using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectFunctionAdapter : IEffectFunction
    {
        private EffectFunction function;

        public EffectFunctionAdapter(EffectFunction function)
        {
            this.function = function;
        }

        #region IEffectFunction Members

        public string Name
        {
            get { return function.Name; }
        }

        #endregion
    }
}
