using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectPassAdapter : IEffectPass
    {
        private EffectPass pass;

        public EffectPassAdapter(EffectPass pass)
        {
            this.pass = pass;
        }

        #region IEffectPass Members

        public ICollectionAdapter<IEffectAnnotation> Annotations
        {
            get { return new EffectAnnotationCollectionAdapter(pass.Annotations); }
        }

        public string Name
        {
            get { return pass.Name; }
        }

        public void Begin()
        {
            pass.Begin();
        }

        public void End()
        {
            pass.End();
        }

        #endregion
    }
}
