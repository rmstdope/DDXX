using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class EffectHandler : IEffectHandler
    {
        private IEffect effect;

        private EffectHandle technique;
        private EffectHandle worldT;
        private EffectHandle worldViewProjT;
        private EffectHandle projT;
        private EffectHandle worldViewT;

        private EffectHandle ambientColor;
        private EffectHandle baseTexture;
        private EffectHandle normalTexture;
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;

        public EffectHandler(IEffect effect)
        {
            technique = effect.FindNextValidTechnique(null);
            CommonInitialize(effect);
        }

        public EffectHandler(IEffect effect, string prefix)
        {
            technique = null;
            while (technique == null || !effect.GetTechniqueName(technique).StartsWith(prefix))
            {
                technique = effect.FindNextValidTechnique(technique);
                if (technique == null)
                    throw new DDXXException("Technique with prefix " + prefix + " not found int effect.");
            }
            CommonInitialize(effect);
        }

        private void CommonInitialize(IEffect effect)
        {
            this.effect = effect;
            worldT = effect.GetParameter(null, "WorldT");
            worldViewProjT = effect.GetParameter(null, "WorldViewProjectionT");
            projT = effect.GetParameter(null, "ProjectionT");
            worldViewT = effect.GetParameter(null, "WorldViewT");

            ambientColor = effect.GetParameter(null, "AmbientColor");
            baseTexture = effect.GetParameter(null, "BaseTexture");
            normalTexture = effect.GetParameter(null, "NormalTexture");
            materialDiffuseColor = effect.GetParameter(null, "MaterialDiffuseColor");
            materialSpecularColor = effect.GetParameter(null, "MaterialSpecularColor");
        }

        #region IEffectHandler Members

        public IEffect Effect
        {
            get { return effect; }
        }

        public EffectHandle Technique
        {
            get { return technique; }
            set { technique = value; }
        }

        public void SetNodeConstants(IRenderableScene scene, INode node)
        {
            effect.Technique = technique;
            if (worldT != null)
                effect.SetValueTranspose(worldT, node.WorldMatrix);
            if (worldViewProjT != null)
                effect.SetValueTranspose(worldViewProjT, node.WorldMatrix * scene.ActiveCamera.ViewMatrix * scene.ActiveCamera.ProjectionMatrix);
            if (projT != null)
                effect.SetValueTranspose(projT, scene.ActiveCamera.ProjectionMatrix);
            if (worldViewT != null)
                effect.SetValueTranspose(worldViewT, node.WorldMatrix * scene.ActiveCamera.ViewMatrix);
        }

        public void SetMaterialConstants(IRenderableScene scene, ModelMaterial material)
        {
            if (ambientColor != null)
                effect.SetValue(ambientColor, ColorOperator.Modulate(scene.AmbientColor, material.AmbientColor));
            if (baseTexture != null)
                effect.SetValue(baseTexture, material.DiffuseTexture);
            if (normalTexture != null)
                effect.SetValue(normalTexture, material.NormalTexture);
            if (materialDiffuseColor != null)
                effect.SetValue(materialDiffuseColor, material.DiffuseColor);
            if (materialSpecularColor != null)
                effect.SetValue(materialSpecularColor, material.SpecularColor);
        }

        #endregion
    }
}
