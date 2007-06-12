using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public delegate string MaterialTechniqueChooser(int material);
    public delegate MaterialTechniqueChooser MeshTechniqueChooser(string meshName);

    public class TechniqueChooser
    {
        public static MaterialTechniqueChooser MaterialPrefix(string prefix)
        {
            return delegate(int material) { return prefix; };
        }
        public static MeshTechniqueChooser MeshPrefix(string prefix)
        {
            return delegate(string name) { return MaterialPrefix(prefix); };
        }
    }

    public class EffectHandler : IEffectHandler
    {
        private IEffect effect;

        private EffectHandle[] techniques;
        private EffectHandle worldT;
        private EffectHandle worldViewProjT;
        private EffectHandle projT;
        private EffectHandle worldViewT;
        private EffectHandle worldViewProjInvT;

        private EffectHandle ambientColor;
        private EffectHandle baseTexture;
        private EffectHandle normalTexture;
        private EffectHandle reflectiveTexture;
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;
        private EffectHandle materialShininess;
        private EffectHandle reflectiveFactor;

        private EffectHandle animationMatrices;

        /// <summary>
        ///  Initialize an EffectHandler without setting any Technique. This must be set
        /// manually by the user or things will crash.
        /// </summary>
        /// <param name="effect"></param>
        public EffectHandler(IEffect effect)
        {
            this.effect = effect;

            CommonInitialize(effect);
        }

        public EffectHandler(IEffect effect, MaterialTechniqueChooser prefix, IModel model)
        {
            this.effect = effect;

            if (model == null)
                techniques = new EffectHandle[1];
            else
                techniques = new EffectHandle[model.Materials.Length];
            for (int i = 0; i < techniques.Length; i++)
            {
                string prefixString = prefix(i);
                techniques[i] = null;
                while (techniques[i] == null || 
                       !effect.GetTechniqueName(techniques[i]).StartsWith(prefixString) || 
                       !ValidateTechnique(model, i))
                {
                    techniques[i] = effect.FindNextValidTechnique(techniques[i]);
                    if (techniques[i] == null)
                        throw new DDXXException("Technique with prefix " + prefixString + " not found in effect.");
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
            worldViewT = effect.GetParameter(null, "WorldViewT");
            worldViewProjT = effect.GetParameter(null, "WorldViewProjectionT");
            worldViewProjInvT = effect.GetParameter(null, "InvWorldViewProjectionT");
            projT = effect.GetParameter(null, "ProjectionT");

            ambientColor = effect.GetParameter(null, "AmbientColor");
            baseTexture = effect.GetParameter(null, "BaseTexture");
            normalTexture = effect.GetParameter(null, "NormalTexture");
            reflectiveTexture = effect.GetParameter(null, "ReflectiveTexture");
            reflectiveFactor = effect.GetParameter(null, "ReflectiveFactor");
            materialDiffuseColor = effect.GetParameter(null, "MaterialDiffuseColor");
            materialSpecularColor = effect.GetParameter(null, "MaterialSpecularColor");
            materialShininess = effect.GetParameter(null, "MaterialShininess");

            animationMatrices = effect.GetParameter(null, "AnimationMatrices");

            if (reflectiveFactor == null)
                throw new DDXXException("Missing mandatory variable ReflectiveFactor in Effect.");
            if (materialShininess == null)
                throw new DDXXException("Missing mandatory variable MaterialShininess in Effect.");
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
            Matrix worldView = worldMatrix * viewMatrix;
            Matrix worldViewProjection = worldView * projectionMatrix;
            Matrix worldViewProjectionInv = worldViewProjection;
            worldViewProjectionInv.Invert();
            if (worldT != null)
                effect.SetValueTranspose(worldT, worldMatrix);
            if (worldViewT != null)
                effect.SetValueTranspose(worldViewT, worldView);
            if (worldViewProjT != null)
                effect.SetValueTranspose(worldViewProjT, worldViewProjection);
            if (worldViewProjInvT != null)
                effect.SetValueTranspose(worldViewProjInvT, worldViewProjectionInv);
            if (projT != null)
                effect.SetValueTranspose(projT, projectionMatrix);
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
            if (reflectiveTexture != null)
                effect.SetValue(reflectiveTexture, material.ReflectiveTexture);
            if (materialDiffuseColor != null)
                effect.SetValue(materialDiffuseColor, material.DiffuseColor);
            if (materialSpecularColor != null)
                effect.SetValue(materialSpecularColor, material.SpecularColor);
            effect.SetValue(materialShininess, material.Shininess);
            effect.SetValue(reflectiveFactor, material.ReflectiveFactor);
        }

        public void SetBones(Matrix[] matrices)
        {
            if (animationMatrices == null)
                throw new DDXXException("Effect does not have AnimationMatrices parameter. SetBones can not be called.");
            effect.SetValue(animationMatrices, matrices);
        }

        #endregion
    }
}
