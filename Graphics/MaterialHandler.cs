using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class MaterialHandler : IMaterialHandler
    {
        private IEffect effect;
        private IEffectConverter effectConverter;
        private BlendFunction blendFunction;
        private Blend sourceBlend;
        private Blend destinationBlend;

        public BlendFunction BlendFunction
        {
            get { return blendFunction; }
            set { blendFunction = value; }
        }

        public Blend SourceBlend
        {
            get { return sourceBlend; }
            set { sourceBlend = value; }
        }

        public Blend DestinationBlend
        {
            get { return destinationBlend; }
            set { destinationBlend = value; }
        }

        public IEffect Effect
        {
            get { return effect; }
            set 
            {
                if (value != null)
                    effectConverter.Convert(effect, value);
                effect = value; 
            }
        }

        public MaterialHandler(IEffect effect, IEffectConverter effectConverter)
        {
            this.effect = effect;
            this.effectConverter = effectConverter;
            this.blendFunction = BlendFunction.Add;
            this.sourceBlend = Blend.One;
            this.destinationBlend = Blend.Zero;
        }

        public void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix,
            Color ambientLight)
        {
            SetupRendering(worldMatrices, viewMatrix, projectionMatrix, ambientLight, new LightState());
        }

        public void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix, 
            Color ambientLight, LightState lightState)
        {
            if (effect is IBasicEffect)
                SetupEffect(worldMatrices, viewMatrix, projectionMatrix, ambientLight, lightState, effect as IBasicEffect);
            else
                SetupEffect(worldMatrices, viewMatrix, projectionMatrix, ambientLight, lightState, effect as IEffect);

            SetupBlending();
        }

        private void SetupBlending()
        {
            if (blendFunction == BlendFunction.Add &&
                sourceBlend == Blend.One &&
                destinationBlend == Blend.Zero)
            {
                effect.GraphicsDevice.RenderState.AlphaBlendEnable = false;
            }
            else
            {
                effect.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                effect.GraphicsDevice.RenderState.BlendFunction = blendFunction;
                effect.GraphicsDevice.RenderState.SourceBlend = sourceBlend;
                effect.GraphicsDevice.RenderState.DestinationBlend = destinationBlend;
            }
        }

        private void SetupEffect(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix, 
            Color ambientLight, LightState lightState, IEffect effect)
        {
            effect.Parameters["World"].SetValue(worldMatrices);
            effect.Parameters["View"].SetValue(viewMatrix);
            effect.Parameters["Projection"].SetValue(projectionMatrix);
            if (lightState != null)
            {
                effect.Parameters["LightPositions"].SetValue(lightState.Positions);
                effect.Parameters["LightDirections"].SetValue(lightState.Directions);
                effect.Parameters["LightDiffuseColors"].SetValue(lightState.DiffuseColor);
                effect.Parameters["LightSpecularColors"].SetValue(lightState.SpecularColor);
            }
            effect.Parameters["AmbientLightColor"].SetValue(ambientLight.ToVector3());
        }

        private void SetupEffect(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix, 
            Color ambientLight, LightState lightState, IBasicEffect effect)
        {
            effect.LightingEnabled = true;
            effect.AmbientLightColor = ambientLight.ToVector3();
            for (int i = 0; i < lightState.NumLights; i++)
            {
                IBasicDirectionalLight light = GetDirectionalLight(effect, i);
                light.Enabled = true;
                light.DiffuseColor = lightState.DiffuseColor[i];
                light.SpecularColor = lightState.DiffuseColor[i];
                light.Direction = lightState.Directions[i];
            }
            for (int i = lightState.NumLights; i < 3; i++)
            {
                GetDirectionalLight(effect, i).Enabled = false;
            }

            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.World = worldMatrices[0];
        }

        private IBasicDirectionalLight GetDirectionalLight(IBasicEffect effect, int i)
        {
            switch (i)
            {
                case 0:
                    return effect.DirectionalLight0;
                case 1:
                    return effect.DirectionalLight1;
            }
            return effect.DirectionalLight2;
        }

        public Color AmbientColor
        {
            get
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("AmbientColor does not work for IBasicEffect effects.");
                return new Color(effect.Parameters["AmbientColor"].GetValueVector3());
            }
            set
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("AmbientColor does not work for IBasicEffect effects.");
                effect.Parameters["AmbientColor"].SetValue(value.ToVector3());
            }
        }

        public Color DiffuseColor
        {
            get
            {
                if (effect is IBasicEffect)
                    return new Color((effect as IBasicEffect).DiffuseColor);
                else
                    return new Color(effect.Parameters["DiffuseColor"].GetValueVector3());
            }
            set
            {
                if (effect is IBasicEffect)
                    (effect as IBasicEffect).DiffuseColor = value.ToVector3();
                else
                    effect.Parameters["DiffuseColor"].SetValue(value.ToVector3());
            }
        }

        public Color SpecularColor
        {
            get
            {
                if (effect is IBasicEffect)
                    return new Color((effect as IBasicEffect).SpecularColor);
                return new Color(effect.Parameters["SpecularColor"].GetValueVector3());
            }
            set
            {
                if (effect is IBasicEffect)
                    (effect as IBasicEffect).SpecularColor = value.ToVector3();
                else
                    effect.Parameters["SpecularColor"].SetValue(value.ToVector3());
            }
        }

        public float SpecularPower
        {
            get
            {
                if (effect is IBasicEffect)
                    return (effect as IBasicEffect).SpecularPower;
                return effect.Parameters["SpecularPower"].GetValueSingle();
            }
            set
            {
                if (effect is IBasicEffect)
                    (effect as IBasicEffect).SpecularPower = value;
                else
                    effect.Parameters["SpecularPower"].SetValue(value);
            }
        }

        public float Shininess
        {
            get
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("Shininess does not work for IBasicEffect effects.");
                return effect.Parameters["Shininess"].GetValueSingle();
            }
            set
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("Shininess does not work for IBasicEffect effects.");
                effect.Parameters["Shininess"].SetValue(value);
            }
        }

        public float Transparency
        {
            get
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("Transparency does not work for IBasicEffect effects.");
                return effect.Parameters["Transparency"].GetValueSingle();
            }
            set
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("Transparency does not work for IBasicEffect effects.");
                effect.Parameters["Transparency"].SetValue(value);
            }
        }

        public ITexture2D DiffuseTexture 
        {
            get
            {
                if (effect is IBasicEffect)
                    return (effect as IBasicEffect).Texture;
                return effect.Parameters["Texture"].GetValueTexture2D();
            }
            set
            {
                if (effect is IBasicEffect)
                {
                    (effect as IBasicEffect).Texture = value;
                    if (value == null)
                        (effect as IBasicEffect).TextureEnabled = false;
                    else
                        (effect as IBasicEffect).TextureEnabled = true;
                }
                else
                    effect.Parameters["Texture"].SetValue(value);
            }
        }

        public ITexture2D NormalTexture
        {
            get
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("NormalTexture does not work for IBasicEffect effects.");
                return effect.Parameters["NormalMap"].GetValueTexture2D();
            }
            set
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("NormalTexture does not work for IBasicEffect effects.");
                effect.Parameters["NormalMap"].SetValue(value);
            }
        }

        public ITextureCube ReflectiveTexture
        {
            get
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("ReflectiveTexture does not work for IBasicEffect effects.");
                return effect.Parameters["ReflectiveMap"].GetValueTextureCube();
            }
            set
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("ReflectiveTexture does not work for IBasicEffect effects.");
                effect.Parameters["ReflectiveMap"].SetValue(value);
            }
        }

        public float ReflectiveFactor
        {
            get
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("ReflectiveFactor does not work for IBasicEffect effects.");
                return effect.Parameters["ReflectiveFactor"].GetValueSingle();
            }
            set
            {
                if (effect is IBasicEffect)
                    throw new DDXXException("ReflectiveFactor does not work for IBasicEffect effects.");
                effect.Parameters["ReflectiveFactor"].SetValue(value);
            }
        }

    }
}
