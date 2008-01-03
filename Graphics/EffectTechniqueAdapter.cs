using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class EffectTechniqueAdapter : IEffectTechnique
    {
        private EffectTechnique technique;

        public EffectTechniqueAdapter(EffectTechnique technique)
        {
            this.technique = technique;
        }

        public EffectTechnique DxEffectTechnique { get { return technique; } }

        #region IEffectTechnique Members

        public ICollectionAdapter<IEffectAnnotation> Annotations
        {
            get { return new EffectAnnotationCollectionAdapter(technique.Annotations); }
        }

        public string Name
        {
            get { return technique.Name; }
        }

        public ICollectionAdapter<IEffectPass> Passes
        {
            get { return new EffectPassCollectionAdapter(technique.Passes); }
        }

        public bool IsParameterUsed(IEffectParameter parameter)
        {
            return technique.IsParameterUsed((parameter as EffectParameterAdapter).DxEffectParameter);
        }

        public bool Validate()
        {
            return technique.Validate();
        }

        #endregion
    }
}
