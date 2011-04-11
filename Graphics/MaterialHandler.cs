using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class MaterialHandler
    {
        private Effect effect;
        private EffectConverter effectConverter;
        private BlendState blendState;
        //private BlendFunction blendFunction;
        //private Blend sourceBlend;
        //private Blend destinationBlend;

        public BlendState BlendState
        {
            get { return blendState; }
            set { blendState = value; }
        }
        //public BlendFunction BlendFunction
        //{
        //    get { return blendFunction; }
        //    set { blendFunction = value; }
        //}

        //public Blend SourceBlend
        //{
        //    get { return sourceBlend; }
        //    set { sourceBlend = value; }
        //}

        //public Blend DestinationBlend
        //{
        //    get { return destinationBlend; }
        //    set { destinationBlend = value; }
        //}

        public Effect Effect
        {
            get { return effect; }
            set 
            {
                if (value != null)
                    effectConverter.Convert(effect, value);
                effect = value; 
            }
        }

        public MaterialHandler(Effect effect, EffectConverter effectConverter)
        {
            this.effect = effect;
            this.effectConverter = effectConverter;
            this.BlendState = new BlendState();
            this.BlendState.ColorBlendFunction = BlendFunction.Add;
            this.BlendState.ColorSourceBlend = Blend.One;
            this.BlendState.ColorDestinationBlend = Blend.Zero;
        }

        public void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix,
            Color ambientLight)
        {
            SetupRendering(worldMatrices, viewMatrix, projectionMatrix, ambientLight, new LightState());
        }

        public void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix, 
            Color ambientLight, LightState lightState)
        {
            if (effect is BasicEffect)
                SetupEffect(worldMatrices, viewMatrix, projectionMatrix, ambientLight, lightState, effect as BasicEffect);
            else
                SetupEffect(worldMatrices, viewMatrix, projectionMatrix, ambientLight, lightState, effect);

            SetupBlending();
        }

        private void SetupBlending()
        {
            effect.GraphicsDevice.BlendState = blendState;
        }

        private void SetupEffect(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrix, 
            Color ambientLight, LightState lightState, Effect effect)
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
            Color ambientLight, LightState lightState, BasicEffect effect)
        {
            effect.LightingEnabled = true;
            effect.AmbientLightColor = ambientLight.ToVector3();
            for (int i = 0; i < lightState.NumLights; i++)
            {
                DirectionalLight light = GetDirectionalLight(effect, i);
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

        private DirectionalLight GetDirectionalLight(BasicEffect effect, int i)
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
                if (effect is BasicEffect)
                    throw new DDXXException("AmbientColor does not work for BasicEffect effects.");
                return new Color(effect.Parameters["AmbientColor"].GetValueVector3());
            }
            set
            {
                if (effect is BasicEffect)
                    throw new DDXXException("AmbientColor does not work for BasicEffect effects.");
                effect.Parameters["AmbientColor"].SetValue(value.ToVector3());
            }
        }

        public Color DiffuseColor
        {
            get
            {
                if (effect is BasicEffect)
                    return new Color((effect as BasicEffect).DiffuseColor);
                else
                    return new Color(effect.Parameters["DiffuseColor"].GetValueVector3());
            }
            set
            {
                if (effect is BasicEffect)
                    (effect as BasicEffect).DiffuseColor = value.ToVector3();
                else
                    effect.Parameters["DiffuseColor"].SetValue(value.ToVector3());
            }
        }

        public Color SpecularColor
        {
            get
            {
                if (effect is BasicEffect)
                    return new Color((effect as BasicEffect).SpecularColor);
                return new Color(effect.Parameters["SpecularColor"].GetValueVector3());
            }
            set
            {
                if (effect is BasicEffect)
                    (effect as BasicEffect).SpecularColor = value.ToVector3();
                else
                    effect.Parameters["SpecularColor"].SetValue(value.ToVector3());
            }
        }

        public float SpecularPower
        {
            get
            {
                if (effect is BasicEffect)
                    return (effect as BasicEffect).SpecularPower;
                return effect.Parameters["SpecularPower"].GetValueSingle();
            }
            set
            {
                if (effect is BasicEffect)
                    (effect as BasicEffect).SpecularPower = value;
                else
                    effect.Parameters["SpecularPower"].SetValue(value);
            }
        }

        public float Shininess
        {
            get
            {
                if (effect is BasicEffect)
                    throw new DDXXException("Shininess does not work for BasicEffect effects.");
                return effect.Parameters["Shininess"].GetValueSingle();
            }
            set
            {
                if (effect is BasicEffect)
                    throw new DDXXException("Shininess does not work for BasicEffect effects.");
                effect.Parameters["Shininess"].SetValue(value);
            }
        }

        public float Transparency
        {
            get
            {
                if (effect is BasicEffect)
                    throw new DDXXException("Transparency does not work for BasicEffect effects.");
                return effect.Parameters["Transparency"].GetValueSingle();
            }
            set
            {
                if (effect is BasicEffect)
                    throw new DDXXException("Transparency does not work for BasicEffect effects.");
                effect.Parameters["Transparency"].SetValue(value);
            }
        }

        public Texture2D DiffuseTexture 
        {
            get
            {
                if (effect is BasicEffect)
                    return (effect as BasicEffect).Texture;
                return effect.Parameters["Texture"].GetValueTexture2D();
            }
            set
            {
                if (effect is BasicEffect)
                {
                    (effect as BasicEffect).Texture = value;
                    if (value == null)
                        (effect as BasicEffect).TextureEnabled = false;
                    else
                        (effect as BasicEffect).TextureEnabled = true;
                }
                else
                    effect.Parameters["Texture"].SetValue(value);
            }
        }

        public Texture2D NormalTexture
        {
            get
            {
                if (effect is BasicEffect)
                    throw new DDXXException("NormalTexture does not work for BasicEffect effects.");
                return effect.Parameters["NormalMap"].GetValueTexture2D();
            }
            set
            {
                if (effect is BasicEffect)
                    throw new DDXXException("NormalTexture does not work for BasicEffect effects.");
                effect.Parameters["NormalMap"].SetValue(value);
            }
        }

        public TextureCube ReflectiveTexture
        {
            get
            {
                if (effect is BasicEffect)
                    throw new DDXXException("ReflectiveTexture does not work for BasicEffect effects.");
                return effect.Parameters["ReflectiveMap"].GetValueTextureCube();
            }
            set
            {
                if (effect is BasicEffect)
                    throw new DDXXException("ReflectiveTexture does not work for BasicEffect effects.");
                effect.Parameters["ReflectiveMap"].SetValue(value);
            }
        }

        public float ReflectiveFactor
        {
            get
            {
                if (effect is BasicEffect)
                    throw new DDXXException("ReflectiveFactor does not work for BasicEffect effects.");
                return effect.Parameters["ReflectiveFactor"].GetValueSingle();
            }
            set
            {
                if (effect is BasicEffect)
                    throw new DDXXException("ReflectiveFactor does not work for BasicEffect effects.");
                effect.Parameters["ReflectiveFactor"].SetValue(value);
            }
        }

    }
}
