using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class EffectHandler : IEffectHandler
    {
        private IEffect effect;

        private EffectHandle[] techniques;
        private EffectHandle worldT;
        private EffectHandle worldViewProjT;
        private EffectHandle projT;
        private EffectHandle worldViewT;

        private EffectHandle ambientColor;
        private EffectHandle baseTexture;
        private EffectHandle normalTexture;
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;

        public EffectHandler(IEffect effect, string prefix, IModel model)
        {
            this.effect = effect;

            if (model == null)
                techniques = new EffectHandle[1];
            else
                techniques = new EffectHandle[model.Materials.Length];
            for (int i = 0; i < techniques.Length; i++)
            {
                techniques[i] = null;
                while (techniques[i] == null || 
                       !effect.GetTechniqueName(techniques[i]).StartsWith(prefix) || 
                       !ValidateTechnique(model, i))
                {
                    techniques[i] = effect.FindNextValidTechnique(techniques[i]);
                    if (techniques[i] == null)
                        throw new DDXXException("Technique with prefix " + prefix + " not found in effect.");
                }
            }

            CommonInitialize(effect);
        }

        private bool ValidateTechnique(IModel model, int index)
        {
            EffectHandle handle;

            handle = effect.GetAnnotation(techniques[index], "Skinning");
            if (handle != null)
            {
                bool skinning = effect.GetValueBoolean(handle);
                if (skinning && !model.IsSkinned())
                    return false;
                if (!skinning && model.IsSkinned())
                    return false;
            }

            handle = effect.GetAnnotation(techniques[index], "NormalMapping");
            if (handle != null)
            {
                bool normalMap = effect.GetValueBoolean(handle);
                if (normalMap && model.Materials[index].NormalTexture == null)
                    return false;
                if (!normalMap && model.Materials[index].NormalTexture != null)
                    return false;
            }

            return true;
        }

        private void CommonInitialize(IEffect effect)
        {
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

        public EffectHandle[] Techniques
        {
            get { return techniques; }
            set { techniques = value; }
        }

        public void SetNodeConstants(Matrix worldMatrix, Matrix viewMatrix, Matrix projectionMatrix)
        {
            if (worldT != null)
                effect.SetValueTranspose(worldT, worldMatrix);
            if (worldViewProjT != null)
                effect.SetValueTranspose(worldViewProjT, worldMatrix * viewMatrix * projectionMatrix);
            if (projT != null)
                effect.SetValueTranspose(projT, projectionMatrix);
            if (worldViewT != null)
                effect.SetValueTranspose(worldViewT, worldMatrix * viewMatrix);
        }

        public void SetMaterialConstants(ColorValue ambientValue, ModelMaterial material, int index)
        {
            effect.Technique = techniques[index];
            if (ambientColor != null)
                effect.SetValue(ambientColor, ColorOperator.Modulate(ambientValue, material.AmbientColor));
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
