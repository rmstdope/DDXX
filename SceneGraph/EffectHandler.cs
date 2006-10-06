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
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;

        public EffectHandler(IEffect effect)
        {
            this.effect = effect;
            technique = effect.FindNextValidTechnique(null);
            
            worldT = effect.GetParameter(null, "WorldT");
            worldViewProjT = effect.GetParameter(null, "WorldViewProjectionT");
            projT = effect.GetParameter(null, "ProjectionT");
            worldViewT = effect.GetParameter(null, "WorldViewT");

            ambientColor = effect.GetParameter(null, "AmbientColor");
            baseTexture = effect.GetParameter(null, "BaseTexture");
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
                effect.SetValue(ambientColor, ColorOperator.Modulate(scene.AmbientColor, material.Material.AmbientColor));
            if (baseTexture != null)
                effect.SetValue(baseTexture, material.DiffuseTexture);
            if (materialDiffuseColor != null)
                effect.SetValue(materialDiffuseColor, material.Material.DiffuseColor);
            if (materialSpecularColor != null)
                effect.SetValue(materialSpecularColor, material.Material.SpecularColor);
        }

        #endregion
    }
}
